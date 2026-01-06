using HMS.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.Json;

namespace HMS.Business.Bus;

public abstract class EventConsumerBackgroundService<T> : BackgroundService
{
    private readonly IEventBus _eventBus;
    private readonly ILogger _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IServiceScopeFactory _scopeFactory;

    protected string EventType { get; set; } = null!;
    protected int PollIntervalMs { get; set; } = 1000;
    protected int MaxRetries { get; set; } = 5;

    protected EventConsumerBackgroundService(
        IEventBus eventBus,
        IServiceScopeFactory scopeFactory,
        ILogger logger)
    {
        _eventBus = eventBus;
        _scopeFactory = scopeFactory;
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("{WorkerName} started. Listening for events of type: {EventType}", GetType().Name, EventType);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _eventBus.FetchPendingAsync(EventType);

                if (result.HasValue)
                {
                    var (eventId, payload) = result.Value;
                    _logger.LogInformation("Event {EventId} of type {EventType} fetched for processing", eventId, EventType);

                    try
                    {
                        var typedPayload = DeserializePayload(payload);
                        if (typedPayload == null)
                        {
                            _logger.LogWarning("Event {EventId} has null payload. Skipping.", eventId);
                            await _eventBus.MarkCompletedAsync(eventId);
                            continue;
                        }

                        await ProcessEventAsync(eventId, typedPayload, stoppingToken);

                        await _eventBus.MarkCompletedAsync(eventId);
                        _logger.LogInformation("Event {EventId} processed successfully", eventId);
                    }
                    catch (Newtonsoft.Json.JsonException jsonEx)
                    {
                        _logger.LogError(jsonEx, "Failed to deserialize payload for event {EventId}", eventId);
                        await _eventBus.MarkFailedAsync(eventId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing event {EventId}", eventId);

                        var appEvent = await GetEventDetails(eventId);
                        var failureCount = appEvent?.FailureCount ?? 0;

                        if (failureCount >= MaxRetries)
                        {
                            _logger.LogWarning("Event {EventId} exceeded max retry count ({MaxRetries}). Marking as failed.", eventId, MaxRetries);
                            await _eventBus.MarkFailedAsync(eventId);
                        }
                        else
                        {
                            await _eventBus.MarkFailedAsync(eventId);
                        }
                    }
                }
                else
                {
                    await Task.Delay(PollIntervalMs, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("{WorkerName} stopping...", GetType().Name);
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {WorkerName}", GetType().Name);
                await Task.Delay(PollIntervalMs, stoppingToken);
            }
        }

        _logger.LogInformation("{WorkerName} stopped", GetType().Name);
    }

    protected abstract Task ProcessEventAsync(Guid eventId, T payload, CancellationToken cancellationToken);

    private T? DeserializePayload(string? payload)
    {
        if (string.IsNullOrEmpty(payload))
            return default;

        return JsonConvert.DeserializeObject<T>(payload);
    }

    private async Task<HMS.Data.DBModel.AppEvent?> GetEventDetails(Guid eventId)
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HMS.Data.DBModel.HMSContext>();
        return await dbContext.AppEvents.FindAsync(eventId);
    }
}
