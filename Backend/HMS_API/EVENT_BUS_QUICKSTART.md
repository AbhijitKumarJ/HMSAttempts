# EventBus Quick Start Guide

## Step 1: Setup Test Database

```bash
psql -U postgres -c "CREATE DATABASE hms_test;"
```

## Step 2: Build the Project

```bash
cd Backend/HMS_API
dotnet build
```

## Step 3: Test the EventBus API

### Publish an Event
```bash
curl -X POST http://localhost:5000/api/Bus/events/publish \
  -H "Content-Type: application/json" \
  -d '{
    "eventType": "PatientRegistered",
    "data": {
      "patientId": 123,
      "mrn": "PAT001",
      "name": "John Doe"
    }
  }'
```

### Fetch Pending Events
```bash
curl http://localhost:5000/api/Bus/events/fetch?eventType=PatientRegistered
```

### Mark Event as Completed
```bash
curl -X POST http://localhost:5000/api/Bus/events/complete \
  -H "Content-Type: application/json" \
  -d "EVENT_ID_HERE"
```

### Mark Event as Failed
```bash
curl -X POST http://localhost:5000/api/Bus/events/fail \
  -H "Content-Type: application/json" \
  -d "EVENT_ID_HERE"
```

## Step 4: Run Unit Tests

```bash
cd Backend/HMS_API
dotnet test HMS.API/HMS.API.csproj --filter "FullyQualifiedName~EventBusTests" --no-build
```

## Step 5: Create a Background Worker

Create `BillingWorker.cs` in `HMS.Business/Workers/`:

```csharp
using HMS.Bus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HMS.Business.Workers;

public class BillingWorker : BackgroundService
{
    private readonly IEventBus _eventBus;
    private readonly ILogger<BillingWorker> _logger;

    public BillingWorker(IEventBus eventBus, ILogger<BillingWorker> logger)
    {
        _eventBus = eventBus;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BillingWorker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _eventBus.FetchPendingAsync("PatientRegistered");

                if (result.HasValue)
                {
                    _logger.LogInformation("Processing event: {EventId}", result.Value.Id);
                    
                    try
                    {
                        // Process the event
                        _logger.LogInformation("Processing payload: {Payload}", result.Value.Payload);
                        
                        // Mark as completed
                        await _eventBus.MarkCompletedAsync(result.Value.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to process event {EventId}", result.Value.Id);
                        await _eventBus.MarkFailedAsync(result.Value.Id);
                    }
                }
                else
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("BillingWorker stopped.");
    }
}
```

## Step 6: Register Worker in Program.cs

Add to `HMS.API/Program.cs`:

```csharp
// Add after other service registrations
builder.Services.AddHostedService<HMS.Business.Workers.BillingWorker>();
```

## Step 7: Verify Everything Works

1. Start the application:
```bash
cd Backend/HMS_API/HMS.API
dotnet run
```

2. In another terminal, publish an event:
```bash
curl -X POST http://localhost:5000/api/Bus/events/publish \
  -H "Content-Type: application/json" \
  -d '{
    "eventType": "PatientRegistered",
    "data": {
      "patientId": 999,
      "mrn": "TEST999"
    }
  }'
```

3. Check the console output - you should see:
   - "BillingWorker started."
   - "Processing event: {EVENT_ID}"
   - "Processing payload: {...}"

## Common Event Types

Here are some suggested event types for your modules:

### Patient Module
- `PatientRegistered`
- `PatientUpdated`
- `EmergencyRegistration`

### Billing Module
- `InvoiceGenerated`
- `PaymentReceived`
- `InvoiceAdjusted`

### Clinical Module
- `ConsultationStarted`
- `ConsultationCompleted`
- `VitalsRecorded`
- `OrderPlaced`

### Inventory Module
- `StockLow`
- `ItemDispensed`
- `RestockNeeded`

### Lab Module
- `OrderCreated`
- `ResultReleased`

## Troubleshooting

### Database Connection Issues
Make sure PostgreSQL is running:
```bash
sudo systemctl status postgresql
```

### Tests Not Running
If you get compilation errors about multiple entry points, try:
```bash
dotnet build HMS.API/HMS.API.csproj -p:GenerateProgramFile=false
dotnet test HMS.API/HMS.API.csproj --no-build
```

### Events Not Being Processed
1. Check that your worker is started (look for "Worker started" in logs)
2. Verify the event type matches exactly
3. Check the event status in the database:
```sql
SELECT * FROM app_events ORDER BY created_at DESC LIMIT 10;
```

## Next Steps

- Create workers for each module
- Implement event processing logic
- Add error handling and retry logic
- Monitor event processing in production
- Consider adding a dashboard to view event queue status

## Resources

- Full Implementation Guide: `EVENT_BUS_IMPLEMENTATION.md`
- Test Documentation: `HMS.API/Tests/Bus/README.md`
- Story Details: `Docs/stories/1-2-shared-message-bus-infrastructure.md`
