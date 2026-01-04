using HMS.Bus;
using HMS.Data.DBModel;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace HMS.API.Tests.Bus;

public class EventBusTests : IAsyncLifetime
{
    private readonly ITestOutputHelper _output;
    private readonly HMSContext _context;
    private readonly IEventBus _eventBus;

    public EventBusTests(ITestOutputHelper output)
    {
        _output = output;

        var options = new DbContextOptionsBuilder<HMSContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=hms_test;Username=postgres;Password=postgres")
            .Options;

        _context = new HMSContext(options);
        _eventBus = new HMS.Bus.EventBus(_context,
            new LoggerFactory().CreateLogger<EventBus>());
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CleanupDatabase();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
    }

    private async Task CleanupDatabase()
    {
        await _context.AppEvents.ExecuteDeleteAsync();
    }

    [Fact]
    public async Task PublishAsync_ShouldCreateEventWithPendingStatus()
    {
        var testPayload = new { patientId = 123, action = "registration" };
        var eventType = "PatientRegistered";

        var eventId = await _eventBus.PublishAsync(eventType, testPayload);

        var dbEvent = await _context.AppEvents.FindAsync(eventId);

        Assert.NotNull(dbEvent);
        Assert.Equal(eventType, dbEvent.EventType);
        Assert.Equal("Pending", dbEvent.Status);
        Assert.NotNull(dbEvent.Payload);
        Assert.NotEqual(Guid.Empty, eventId);
    }

    [Fact]
    public async Task PublishAsync_ShouldSerializePayloadToJson()
    {
        var testPayload = new { orderId = 456, items = new[] { "Medication A", "Medication B" } };
        var eventType = "OrderPlaced";

        var eventId = await _eventBus.PublishAsync(eventType, testPayload);
        var dbEvent = await _context.AppEvents.FindAsync(eventId);

        Assert.NotNull(dbEvent.Payload);
        Assert.Contains("orderId", dbEvent.Payload);
    }

    [Fact]
    public async Task FetchPendingAsync_ShouldReturnNullWhenNoPendingEvents()
    {
        var eventType = "NonExistentEvent";

        var result = await _eventBus.FetchPendingAsync(eventType);

        Assert.Null(result);
    }

    [Fact]
    public async Task FetchPendingAsync_ShouldUpdateStatusToProcessing()
    {
        var testPayload = new { taskId = 789 };
        var eventType = "TaskCreated";

        await _eventBus.PublishAsync(eventType, testPayload);
        var result = await _eventBus.FetchPendingAsync(eventType);

        Assert.NotNull(result);
        Assert.NotNull(result.Value.Payload);

        var dbEvent = await _context.AppEvents.FindAsync(result.Value.Id);
        Assert.Equal("Processing", dbEvent.Status);
        Assert.NotNull(dbEvent.ProcessedAt);
    }

    [Fact]
    public async Task FetchPendingAsync_ShouldReturnOldestEventFirst()
    {
        var eventType = "OrderedEvent";

        await _eventBus.PublishAsync(eventType, new { id = 1 });
        await Task.Delay(50);
        await _eventBus.PublishAsync(eventType, new { id = 2 });
        await Task.Delay(50);
        await _eventBus.PublishAsync(eventType, new { id = 3 });

        var firstResult = await _eventBus.FetchPendingAsync(eventType);
        var payload1 = Newtonsoft.Json.Linq.JToken.Parse(firstResult.Value.Payload!);
        var firstId = (int?)payload1["id"];

        Assert.Equal(1, firstId);
    }

    [Fact]
    public async Task MarkCompletedAsync_ShouldUpdateStatusToCompleted()
    {
        var testPayload = new { testId = 1 };
        var eventType = "TestEvent";

        var eventId = await _eventBus.PublishAsync(eventType, testPayload);
        await _eventBus.MarkCompletedAsync(eventId);

        var dbEvent = await _context.AppEvents.FindAsync(eventId);
        Assert.Equal("Completed", dbEvent.Status);
        Assert.NotNull(dbEvent.ProcessedAt);
    }

    [Fact]
    public async Task MarkFailedAsync_ShouldIncrementFailureCount()
    {
        var testPayload = new { testId = 1 };
        var eventType = "FailingEvent";

        var eventId = await _eventBus.PublishAsync(eventType, testPayload);

        await _eventBus.MarkFailedAsync(eventId);
        var dbEvent = await _context.AppEvents.FindAsync(eventId);
        Assert.Equal("Failed", dbEvent.Status);
        Assert.Equal(1, dbEvent.FailureCount);

        await _eventBus.MarkFailedAsync(eventId);
        dbEvent = await _context.AppEvents.FindAsync(eventId);
        Assert.Equal(2, dbEvent.FailureCount);
    }

    [Fact]
    public async Task ConcurrencyFetch_ShouldNotReturnSameEventToMultipleWorkers()
    {
        var eventType = "ConcurrencyTest";
        var payload = new { testId = 999 };

        await _eventBus.PublishAsync(eventType, payload);

        var tasks = new List<Task<(Guid Id, string? Payload)?>>();
        for (int i = 0; i < 3; i++)
        {
            tasks.Add(_eventBus.FetchPendingAsync(eventType));
        }

        var results = await Task.WhenAll(tasks);
        var nonNullResults = results.Where(r => r.HasValue).Select(r => r.Value.Id).Distinct().ToList();

        Assert.Single(nonNullResults);
        Assert.Equal(1, nonNullResults.Count);
    }

    [Fact]
    public async Task ConcurrencyFetch_WithMultipleEvents_ShouldDistributeEvents()
    {
        var eventType = "DistributedEvent";
        var eventCount = 3;

        for (int i = 0; i < eventCount; i++)
        {
            await _eventBus.PublishAsync(eventType, new { id = i });
        }

        var tasks = new List<Task<(Guid Id, string? Payload)?>>();
        for (int i = 0; i < eventCount; i++)
        {
            tasks.Add(_eventBus.FetchPendingAsync(eventType));
        }

        var results = await Task.WhenAll(tasks);
        var fetchedIds = results.Where(r => r.HasValue).Select(r => r.Value.Id).Distinct().ToList();

        Assert.Equal(eventCount, fetchedIds.Count);
    }

    [Fact]
    public async Task EndToEnd_ShouldProcessEventThroughLifecycle()
    {
        var eventType = "EndToEndTest";
        var payload = new { workflowId = "TEST-001", step = "initial" };

        var eventId = await _eventBus.PublishAsync(eventType, payload);
        var fetchedEvent = await _eventBus.FetchPendingAsync(eventType);

        Assert.NotNull(fetchedEvent);
        Assert.Equal(eventId, fetchedEvent.Value.Id);

        var deserializedPayload = Newtonsoft.Json.Linq.JToken.Parse(fetchedEvent.Value.Payload!);
        Assert.Equal("TEST-001", deserializedPayload["workflowId"]?.ToString());

        await _eventBus.MarkCompletedAsync(eventId);

        var completedEvent = await _context.AppEvents.FindAsync(eventId);
        Assert.Equal("Completed", completedEvent.Status);
        Assert.NotNull(completedEvent.ProcessedAt);
    }

    [Fact]
    public async Task FetchPendingAsync_ShouldFilterByEventType()
    {
        await _eventBus.PublishAsync("TypeA", new { id = 1 });
        await _eventBus.PublishAsync("TypeB", new { id = 2 });
        await _eventBus.PublishAsync("TypeA", new { id = 3 });

        var typeAFetch = await _eventBus.FetchPendingAsync("TypeA");
        var payloadA = Newtonsoft.Json.Linq.JToken.Parse(typeAFetch.Value.Payload!);
        var idA = (int?)payloadA["id"];

        var typeBFetch = await _eventBus.FetchPendingAsync("TypeB");
        var payloadB = Newtonsoft.Json.Linq.JToken.Parse(typeBFetch.Value.Payload!);
        var idB = (int?)payloadB["id"];

        Assert.Equal(1, idA);
        Assert.Equal(2, idB);
    }
}
