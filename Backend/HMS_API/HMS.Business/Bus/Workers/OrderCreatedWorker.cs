using HMS.Bus;
using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace HMS.Business.Bus.Workers;

public record OrderCreatedEvent(int OrderId, int PatientId, string OrderType, string Description);

public class OrderCreatedWorker : EventConsumerBackgroundService<OrderCreatedEvent>
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<OrderCreatedWorker> _logger;

    public OrderCreatedWorker(
        IEventBus eventBus,
        IServiceScopeFactory scopeFactory,
        ILogger<OrderCreatedWorker> logger)
        : base(eventBus, scopeFactory, logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        EventType = "Order.Created";
    }

    protected override async Task ProcessEventAsync(Guid eventId, OrderCreatedEvent payload, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HMSContext>();

        var existingOrder = await context.LabResults
            .FirstOrDefaultAsync(lr => lr.OrderId == payload.OrderId, cancellationToken);

        if (existingOrder != null)
        {
            _logger.LogWarning("Lab order for OrderId {OrderId} already exists. Skipping idempotent processing.", payload.OrderId);
            return;
        }

        var labResult = new LabResult
        {
            OrderId = payload.OrderId,
            ResultSummary = $"Order created: {payload.OrderType}",
            ResultData = Newtonsoft.Json.Linq.JObject.FromObject(new
            {
                orderType = payload.OrderType,
                description = payload.Description,
                patientId = payload.PatientId,
                createdAt = DateTime.UtcNow
            }).ToString(),
            ReleasedAt = null
        };

        context.LabResults.Add(labResult);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created LabResult for OrderId {OrderId}", payload.OrderId);
    }
}
