using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HMS.Bus;

public class EventBus : IEventBus
{
    private readonly HMSContext _context;
    private readonly ILogger<EventBus> _logger;

    public EventBus(HMSContext context, ILogger<EventBus> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> PublishAsync(string eventType, object payload)
    {
        var jsonPayload = JsonConvert.SerializeObject(payload);
        var appEvent = new AppEvent
        {
            EventType = eventType,
            Payload = jsonPayload,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _context.AppEvents.Add(appEvent);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Published event {EventId} of type {EventType}", appEvent.Id, eventType);
        return appEvent.Id;
    }

    public async Task<(Guid Id, string? Payload)?> FetchPendingAsync(string eventType)
    {
        var sql = @"
            UPDATE app_events
            SET status = 'Processing', processed_at = NOW()
            WHERE id = (
                SELECT id
                FROM app_events
                WHERE status = 'Pending' AND event_type = @type
                ORDER BY created_at ASC
                LIMIT 1
                FOR UPDATE SKIP LOCKED
            )
            RETURNING id, payload;";

        var result = await _context.Database
            .SqlQueryRaw<(Guid Id, string? Payload)>(sql, new Npgsql.NpgsqlParameter("type", eventType))
            .FirstOrDefaultAsync();

        if (result.Id != Guid.Empty)
        {
            _logger.LogInformation("Fetched event {EventId} of type {EventType}", result.Id, eventType);
        }

        return result.Id != Guid.Empty ? result : null;
    }

    public async Task MarkCompletedAsync(Guid id)
    {
        var appEvent = await _context.AppEvents.FindAsync(id);
        if (appEvent != null)
        {
            appEvent.Status = "Completed";
            appEvent.ProcessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Marked event {EventId} as Completed", id);
        }
    }

    public async Task MarkFailedAsync(Guid id)
    {
        var appEvent = await _context.AppEvents.FindAsync(id);
        if (appEvent != null)
        {
            appEvent.Status = "Failed";
            appEvent.FailureCount = (appEvent.FailureCount ?? 0) + 1;
            appEvent.ProcessedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            _logger.LogWarning("Marked event {EventId} as Failed (attempt {Attempt})", id, appEvent.FailureCount);
        }
    }
}
