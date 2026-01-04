# EventBus Implementation

## Overview
This implementation provides a shared message bus infrastructure using PostgreSQL for asynchronous module communication.

## Implementation Details

### 1. IEventBus Interface (`HMS.Business/IEventBus.cs`)
- `PublishAsync(string eventType, object payload)` - Publishes events to the message bus
- `FetchPendingAsync(string eventType)` - Fetches pending events using SKIP LOCKED for concurrency
- `MarkCompletedAsync(Guid id)` - Marks event as successfully processed
- `MarkFailedAsync(Guid id)` - Marks event as failed and increments failure count

### 2. EventBus Service (`HMS.Business/EventBus.cs`)
- Uses Newtonsoft.Json for payload serialization (matching project standard)
- Implements PostgreSQL SKIP LOCKED pattern for concurrent consumption
- Comprehensive logging for all operations

### 3. BusController Updates (`HMS.API/Controllers/Bus/BusController.cs`)
Added EventBus endpoints:

- `POST /api/Bus/events/publish` - Publish a new event
  ```json
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "eventType": "PatientRegistered",
    "data": { "patientId": 123, "action": "registration" }
  }
  ```

- `GET /api/Bus/events/fetch?eventType=PatientRegistered` - Fetch pending event

- `POST /api/Bus/events/complete` - Mark event as completed
  ```json
  "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ```

- `POST /api/Bus/events/fail` - Mark event as failed
  ```json
  "3fa85f64-5717-4562-b3fc-2c963f66afa6"
  ```

## Database Schema

The `app_events` table is automatically created by EF Core migrations:

```sql
CREATE TABLE app_events (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    event_type VARCHAR(150) NOT NULL,
    payload JSONB,
    status VARCHAR(50) DEFAULT 'Pending',
    created_at TIMESTAMP DEFAULT NOW(),
    processed_at TIMESTAMP,
    failure_count INT DEFAULT 0
);
CREATE INDEX idx_app_events_event_type ON app_events(event_type);
```

## Concurrency Safety

The implementation uses PostgreSQL's `FOR UPDATE SKIP LOCKED` pattern:

```sql
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
RETURNING id, payload;
```

This allows multiple worker instances to poll the table simultaneously without blocking each other or processing the same row.

## Running Tests

### Setup Test Database

```bash
# Create test database
psql -U postgres -c "CREATE DATABASE hms_test;"

# Or use the connection string in the test files:
# Host=localhost;Port=5432;Database=hms_test;Username=postgres;Password=postgres
```

### Run Tests

```bash
# Run all tests
dotnet test HMS.API/HMS.API.csproj

# Run specific test class
dotnet test HMS.API/HMS.API.csproj --filter "FullyQualifiedName~EventBusTests"

# Run specific test
dotnet test HMS.API/HMS.API.csproj --filter "FullyQualifiedName~PublishAsync_ShouldCreateEventWithPendingStatus"

# Run with verbose output
dotnet test HMS.API/HMS.API.csproj --logger "console;verbosity=detailed"
```

## Test Coverage

### Unit Tests (`HMS.API/Tests/Bus/EventBusTests.cs`)

1. **PublishAsync_ShouldCreateEventWithPendingStatus**
   - Verifies events are created with correct status

2. **PublishAsync_ShouldSerializePayloadToJson**
   - Validates JSON serialization of payloads

3. **FetchPendingAsync_ShouldReturnNullWhenNoPendingEvents**
   - Tests handling of empty queue

4. **FetchPendingAsync_ShouldUpdateStatusToProcessing**
   - Ensures status transition during fetch

5. **FetchPendingAsync_ShouldReturnOldestEventFirst**
   - Validates FIFO ordering

6. **MarkCompletedAsync_ShouldUpdateStatusToCompleted**
   - Tests successful completion marking

7. **MarkFailedAsync_ShouldIncrementFailureCount**
   - Validates failure tracking

8. **ConcurrencyFetch_ShouldNotReturnSameEventToMultipleWorkers**
   - **Critical**: Tests SKIP LOCKED concurrency safety

9. **ConcurrencyFetch_WithMultipleEvents_ShouldDistributeEvents**
   - Tests concurrent distribution of multiple events

10. **EndToEnd_ShouldProcessEventThroughLifecycle**
    - Full lifecycle test

11. **FetchPendingAsync_ShouldFilterByEventType**
    - Validates event type filtering

## Usage Example

### Publishing an Event

```csharp
public class OrderService
{
    private readonly IEventBus _eventBus;

    public OrderService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task CreateOrder(Order order)
    {
        // Save order to database
        await _orderRepository.AddAsync(order);

        // Publish event for other modules
        await _eventBus.PublishAsync("OrderCreated", new
        {
            orderId = order.Id,
            patientId = order.PatientId,
            items = order.Items,
            totalAmount = order.TotalAmount
        });
    }
}
```

### Consuming Events (Background Worker)

```csharp
public class OrderProcessingWorker : BackgroundService
{
    private readonly IEventBus _eventBus;

    public OrderProcessingWorker(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await _eventBus.FetchPendingAsync("OrderCreated");
            
            if (result.HasValue)
            {
                try
                {
                    await ProcessOrder(result.Value.Payload);
                    await _eventBus.MarkCompletedAsync(result.Value.Id);
                }
                catch (Exception ex)
                {
                    await _eventBus.MarkFailedAsync(result.Value.Id);
                }
            }
            else
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    private async Task ProcessOrder(string payload)
    {
        // Process the order
    }
}
```

## Registration

The EventBus is automatically registered in `Program.cs`:

```csharp
builder.Services.AddScoped<IEventBus, EventBus>();
```

For background workers, you can add:

```csharp
builder.Services.AddHostedService<OrderProcessingWorker>();
```

## Notes

- All datetime operations use UTC
- Payloads are serialized using Newtonsoft.Json (matching project standards)
- Event IDs are UUIDs for distributed system compatibility
- Failed events can be retried by resetting their status to 'Pending'
