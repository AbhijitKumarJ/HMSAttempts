# Background Workers Quickstart Guide

This guide shows you how to create and register background workers (event consumers) for the HMS EventBus system.

## Overview

Background workers are long-running services that:
- Poll the `app_events` table for specific event types
- Process events asynchronously
- Handle retries and errors automatically
- Provide idempotency guarantees

## Creating a New Worker

### Step 1: Define Your Event Payload

Create a C# record or class that represents your event data:

```csharp
namespace HMS.Business.Bus.Workers;

public record PatientAdmittedEvent(
    int PatientId,
    string Mrn,
    DateTime AdmittedAt,
    string Department
);
```

### Step 2: Create the Worker Class

Inherit from `EventConsumerBackgroundService<T>` and implement the abstract method:

```csharp
using HMS.Bus;
using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace HMS.Business.Bus.Workers;

public class PatientAdmittedWorker : EventConsumerBackgroundService<PatientAdmittedEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public PatientAdmittedWorker(
        IEventBus eventBus,
        IServiceProvider serviceProvider,
        ILogger<PatientAdmittedWorker> logger)
        : base(eventBus, logger)
    {
        _serviceProvider = serviceProvider;
        EventType = "Patient.Admitted"; // Must match event type published to bus
    }

    protected override async Task ProcessEventAsync(
        Guid eventId,
        PatientAdmittedEvent payload,
        CancellationToken cancellationToken)
    {
        // Create a new scope for database operations
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HMSContext>();

        // YOUR BUSINESS LOGIC HERE
        // Example: Create admission record
        var admission = new ClinConsultation
        {
            PatientId = payload.PatientId,
            StartedAt = payload.AdmittedAt,
            ClinicalSummary = $"Admitted to {payload.Department}"
        };

        context.ClinConsultations.Add(admission);
        await context.SaveChangesAsync(cancellationToken);

        // Idempotency check (optional but recommended)
        // See "Best Practices" section below
    }
}
```

### Step 3: Register the Worker

Add the worker to `Program.cs`:

```csharp
// In Program.cs, after service registrations
builder.Services.AddHostedService<PatientAdmittedWorker>();
```

### Step 4: Publish Events

From any service, publish events that your worker will consume:

```csharp
public class PatientService
{
    private readonly IEventBus _eventBus;

    public async Task AdmitPatient(int patientId, string department)
    {
        // Save patient to database
        // ...

        // Publish event for background processing
        await _eventBus.PublishAsync("Patient.Admitted", new PatientAdmittedEvent(
            patientId,
            "MRN-001",
            DateTime.UtcNow,
            department
        ));
    }
}
```

## Configuration Options

All workers inherit these configurable properties:

| Property | Default | Description |
|----------|---------|-------------|
| `EventType` | (Required) | The event type to listen for in `app_events` table |
| `PollIntervalMs` | 1000 | How often to poll for new events (milliseconds) |
| `MaxRetries` | 5 | Maximum number of retry attempts before marking as failed |

Override in constructor:

```csharp
public class FastPollingWorker : EventConsumerBackgroundService<TestEvent>
{
    public FastPollingWorker(IEventBus eventBus, ILogger<FastPollingWorker> logger)
        : base(eventBus, logger)
    {
        EventType = "Test.Event";
        PollIntervalMs = 500; // Poll every 500ms instead of 1000ms
        MaxRetries = 3;      // Fail after 3 retries instead of 5
    }
}
```

## Best Practices

### 1. Use Service Scopes for Database Operations

Always create a new scope for database operations:

```csharp
protected override async Task ProcessEventAsync(Guid eventId, T payload, CancellationToken cancellationToken)
{
    using var scope = _serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<HMSContext>();

    // Database operations here
}
```

### 2. Implement Idempotency

Prevent duplicate processing:

```csharp
protected override async Task ProcessEventAsync(Guid eventId, PatientAdmittedEvent payload, CancellationToken cancellationToken)
{
    using var scope = _serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<HMSContext>();

    // Check if already processed
    var existing = await context.ClinConsultations
        .AnyAsync(c => c.PatientId == payload.PatientId 
            && c.StartedAt == payload.AdmittedAt, 
            cancellationToken);

    if (existing)
    {
        Logger.LogWarning("Event {EventId} already processed, skipping", eventId);
        return;
    }

    // Process event
}
```

Or use `SourceEventId` pattern:

```csharp
public class MedicationDispensedWorker : EventConsumerBackgroundService<MedicationDispensedEvent>
{
    protected override async Task ProcessEventAsync(Guid eventId, MedicationDispensedEvent payload, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<HMSContext>();

        // Check if invoice item already exists for this event
        var existingItem = await context.BilInvoiceItems
            .FirstOrDefaultAsync(ii => ii.SourceEventId == eventId.ToString(), 
                cancellationToken);

        if (existingItem != null)
        {
            Logger.LogWarning("InvoiceItem for EventId {EventId} already exists", eventId);
            return;
        }

        // Create new item with SourceEventId for idempotency
        var invoiceItem = new BilInvoiceItem
        {
            // ... other properties ...
            SourceEventId = eventId.ToString()
        };

        context.BilInvoiceItems.Add(invoiceItem);
        await context.SaveChangesAsync(cancellationToken);
    }
}
```

### 3. Use Transactions for Multi-Step Operations

Wrap multiple database operations in a transaction:

```csharp
protected override async Task ProcessEventAsync(Guid eventId, MyEvent payload, CancellationToken cancellationToken)
{
    using var scope = _serviceProvider.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<HMSContext>();

    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

    try
    {
        // Operation 1
        var invoice = new BilInvoice { /* ... */ };
        context.BilInvoices.Add(invoice);
        await context.SaveChangesAsync(cancellationToken);

        // Operation 2
        var item = new BilInvoiceItem { InvoiceId = invoice.Id, /* ... */ };
        context.BilInvoiceItems.Add(item);
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
    catch
    {
        await transaction.RollbackAsync(cancellationToken);
        throw; // Let base class handle retry logic
    }
}
```

### 4. Validate Input Early

```csharp
protected override async Task ProcessEventAsync(Guid eventId, MyEvent payload, CancellationToken cancellationToken)
{
    if (payload.PatientId <= 0)
    {
        Logger.LogError("Invalid PatientId in event {EventId}: {PatientId}", eventId, payload.PatientId);
        throw new ArgumentException("Invalid PatientId", nameof(payload.PatientId));
    }

    // Continue processing
}
```

### 5. Log Important Operations

```csharp
protected override async Task ProcessEventAsync(Guid eventId, MyEvent payload, CancellationToken cancellationToken)
{
    Logger.LogInformation("Processing event {EventId} for PatientId {PatientId}", eventId, payload.PatientId);

    try
    {
        // Process
        Logger.LogInformation("Successfully processed event {EventId}", eventId);
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "Failed to process event {EventId}", eventId);
        throw;
    }
}
```

## Error Handling

The base class automatically handles:

1. **JSON Deserialization Errors**: Events with invalid JSON are marked as Failed
2. **Processing Errors**: Events are retried up to `MaxRetries` times
3. **Permanent Failures**: After max retries, events are marked as Failed (dead letter)

You only need to:
- Throw exceptions for business logic errors
- Let the base class handle retry logic

### Custom Error Handling

If you need custom error handling:

```csharp
protected override async Task ProcessEventAsync(Guid eventId, MyEvent payload, CancellationToken cancellationToken)
{
    try
    {
        // Your logic
    }
    catch (SpecificBusinessException ex)
    {
        Logger.LogError(ex, "Specific business error for event {EventId}", eventId);
        // Don't throw - this event will be marked as completed anyway
    }
    catch (Exception ex)
    {
        // Unknown errors trigger retry logic
        Logger.LogError(ex, "Unexpected error processing event {EventId}", eventId);
        throw;
    }
}
```

## Testing Workers

Create test consumers with configurable behavior:

```csharp
public record TestEvent(int Id, string Name);

public class TestEventConsumer : EventConsumerBackgroundService<TestEvent>
{
    public int ProcessCallCount { get; private set; }
    public Exception? ThrowException { get; set; }

    public TestEventConsumer(IEventBus eventBus, ILogger<TestEventConsumer> logger)
        : base(eventBus, logger)
    {
        EventType = "Test.Event";
    }

    protected override async Task ProcessEventAsync(Guid eventId, TestEvent payload, CancellationToken cancellationToken)
    {
        ProcessCallCount++;
        if (ThrowException != null)
            throw ThrowException;
    }
}
```

Mock `IEventBus` in tests:

```csharp
[Fact]
public async Task ShouldProcessEventSuccessfully()
{
    var mockEventBus = new Mock<IEventBus>();
    var mockLogger = new Mock<ILogger<TestEventConsumer>>();
    var consumer = new TestEventConsumer(mockEventBus.Object, mockLogger.Object);

    var eventId = Guid.NewGuid();
    var payload = new TestEvent(1, "Test");

    mockEventBus
        .Setup(x => x.FetchPendingAsync("Test.Event"))
        .ReturnsAsync((eventId, Newtonsoft.Json.JsonConvert.SerializeObject(payload)));

    mockEventBus
        .Setup(x => x.MarkCompletedAsync(eventId))
        .Returns(Task.CompletedTask);

    var cts = new CancellationTokenSource();
    cts.CancelAfter(200);

    await consumer.ExecuteAsync(cts.Token);

    Assert.Equal(1, consumer.ProcessCallCount);
}
```

## Troubleshooting

### Worker Not Starting
- Check that `AddHostedService` is called in `Program.cs`
- Verify no exceptions in worker constructor
- Check application logs for startup errors

### Events Not Being Processed
- Verify `EventType` matches the published event type exactly
- Check that events have status "Pending" in `app_events` table
- Ensure worker is registered and running
- Check worker logs for error messages

### Events Being Retried Indefinitely
- Check `MaxRetries` configuration
- Review exceptions being thrown in `ProcessEventAsync`
- Ensure business logic is idempotent

### Database Connection Issues
- Worker uses separate scopes, but needs correct connection string
- Verify DbContext is properly registered in DI
- Check database permissions

## Example Workers

See these files for real-world examples:

1. `HMS.Business/Bus/Workers/OrderCreatedWorker.cs`
   - Creates LabOrder records
   - Demonstrates idempotency check

2. `HMS.Business/Bus/Workers/MedicationDispensedWorker.cs`
   - Triggers billing workflow
   - Creates or finds active invoice
   - Uses transactional updates

## Architecture Diagram

```
┌──────────────────────────────────────────────────────────┐
│                  EventConsumerBackgroundService<T>         │
│  ┌────────────────────────────────────────────────────┐  │
│  │  BackgroundService.ExecuteAsync()                   │  │
│  │  - Poll app_events table                          │  │
│  │  - Deserialize payload to T                         │  │
│  │  - Call ProcessEventAsync()                        │  │
│  │  - Handle errors & retries                         │  │
│  │  - Mark Completed/Failed                           │  │
│  └────────────────────────────────────────────────────┘  │
│                        ↑                                │
│  ┌────────────────────────────────────────────────────┐  │
│  │  abstract ProcessEventAsync()                    │  │
│  │  Your worker implements this                      │  │
│  └────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────┘
            ↑                              ↑
            │                              │
    ┌───────┴────────┐         ┌────────┴──────────┐
    │ YourWorker1     │         │ YourWorker2        │
    │ ProcessEventAsync│         │ ProcessEventAsync   │
    └─────────────────┘         └───────────────────┘
```

## Next Steps

1. Create your worker class following the template above
2. Implement business logic in `ProcessEventAsync`
3. Add idempotency checks
4. Register in `Program.cs`
5. Write unit tests
6. Deploy and monitor logs

## Related Documentation

- [EventBus Implementation Guide](../IMPL_Guides/EVENT_BUS_IMPLEMENTATION.md)
- [EventBus Tests](../HMS.Tests/Tests/Bus/)
