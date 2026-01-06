using HMS.Bus;
using HMS.Business.Bus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace HMS.Tests.Bus;

public record TestEvent(int Id, string Name, DateTime Timestamp);

public class TestEventConsumer : EventConsumerBackgroundService<TestEvent>
{
    public int ProcessCallCount { get; private set; }
    public TestEvent? LastProcessedEvent { get; private set; }
    public Guid? LastEventId { get; private set; }
    public Exception? ThrowExceptionOnEvent { get; set; }
    public int ProcessingDelayMs { get; set; }

    public TestEventConsumer(
        IEventBus eventBus,
        IServiceScopeFactory scopeFactory,
        ILogger<TestEventConsumer> logger)
        : base(eventBus, scopeFactory, logger)
    {
        EventType = "Test.Event";
    }

    protected override async Task ProcessEventAsync(Guid eventId, TestEvent payload, CancellationToken cancellationToken)
    {
        LastEventId = eventId;
        LastProcessedEvent = payload;
        ProcessCallCount++;

        if (ProcessingDelayMs > 0)
        {
            await Task.Delay(ProcessingDelayMs, cancellationToken);
        }

        if (ThrowExceptionOnEvent != null)
        {
            throw ThrowExceptionOnEvent;
        }
    }
}

public class EventConsumerBackgroundServiceTests
{
    private readonly Mock<IEventBus> _mockEventBus;
    private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
    private readonly Mock<ILogger<TestEventConsumer>> _mockLogger;
    private readonly TestEventConsumer _consumer;

    public EventConsumerBackgroundServiceTests()
    {
        _mockEventBus = new Mock<IEventBus>();
        _mockScopeFactory = new Mock<IServiceScopeFactory>();
        _mockLogger = new Mock<ILogger<TestEventConsumer>>();
        _consumer = new TestEventConsumer(_mockEventBus.Object, _mockScopeFactory.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessEventsSuccessfully()
    {
        var eventId = Guid.NewGuid();
        var testEvent = new TestEvent(1, "Test", DateTime.UtcNow);
        var payload = Newtonsoft.Json.JsonConvert.SerializeObject(testEvent);

        var result = new ValueTuple<Guid, string?>(eventId, payload);
        _mockEventBus
            .Setup(x => x.FetchPendingAsync("Test.Event"))
            .ReturnsAsync(result);

        _mockEventBus
            .Setup(x => x.MarkCompletedAsync(eventId))
            .Returns(Task.CompletedTask);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(200);

        await _consumer.ExecuteAsync(cts.Token);

        Assert.Equal(1, _consumer.ProcessCallCount);
        Assert.Equal(testEvent.Id, _consumer.LastProcessedEvent?.Id);
        Assert.Equal(eventId, _consumer.LastEventId);

        _mockEventBus.Verify(x => x.FetchPendingAsync("Test.Event"), Times.Once);
        _mockEventBus.Verify(x => x.MarkCompletedAsync(eventId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRetryOnProcessingFailure()
    {
        var eventId = Guid.NewGuid();
        var testEvent = new TestEvent(1, "Test", DateTime.UtcNow);
        var payload = Newtonsoft.Json.JsonConvert.SerializeObject(testEvent);
        var exception = new InvalidOperationException("Processing failed");

        var result = new ValueTuple<Guid, string?>(eventId, payload);
        _mockEventBus
            .Setup(x => x.FetchPendingAsync("Test.Event"))
            .ReturnsAsync(result);

        _mockEventBus
            .Setup(x => x.MarkFailedAsync(eventId))
            .Returns(Task.CompletedTask);

        _consumer.ThrowExceptionOnEvent = exception;
        _consumer.MaxRetries = 2;

        var cts = new CancellationTokenSource();
        cts.CancelAfter(200);

        await _consumer.ExecuteAsync(cts.Token);

        _mockEventBus.Verify(x => x.MarkFailedAsync(eventId), Times.AtLeastOnce);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldPollWhenNoEventsAvailable()
    {
        _mockEventBus
            .Setup(x => x.FetchPendingAsync("Test.Event"))
            .ReturnsAsync(default((Guid, string?)));

        _consumer.PollIntervalMs = 50;

        var cts = new CancellationTokenSource();
        cts.CancelAfter(150);

        await _consumer.ExecuteAsync(cts.Token);

        _mockEventBus.Verify(x => x.FetchPendingAsync("Test.Event"), Times.AtLeast(2));
        Assert.Equal(0, _consumer.ProcessCallCount);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldHandleJsonDeserializationError()
    {
        var eventId = Guid.NewGuid();
        var invalidPayload = "{ invalid json }";

        var result = new ValueTuple<Guid, string?>(eventId, invalidPayload);
        _mockEventBus
            .Setup(x => x.FetchPendingAsync("Test.Event"))
            .ReturnsAsync(result);

        _mockEventBus
            .Setup(x => x.MarkFailedAsync(eventId))
            .Returns(Task.CompletedTask);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(200);

        await _consumer.ExecuteAsync(cts.Token);

        _mockEventBus.Verify(x => x.MarkFailedAsync(eventId), Times.Once);
        Assert.Equal(0, _consumer.ProcessCallCount);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldStopGracefullyOnCancellation()
    {
        var eventId1 = Guid.NewGuid();
        var eventId2 = Guid.NewGuid();
        var testEvent = new TestEvent(1, "Test", DateTime.UtcNow);
        var payload = Newtonsoft.Json.JsonConvert.SerializeObject(testEvent);

        var callCount = 0;
        var result1 = new ValueTuple<Guid, string?>(eventId1, payload);
        var result2 = new ValueTuple<Guid, string?>(eventId2, payload);
        
        _mockEventBus
            .Setup(x => x.FetchPendingAsync("Test.Event"))
            .Returns(() =>
            {
                callCount++;
                if (callCount == 1)
                    return Task.FromResult<(Guid, string?)?>result1;
                else if (callCount == 2)
                    return Task.FromResult<(Guid, string?)?>result2;
                else
                    return Task.FromResult<(Guid, string?)?>(default);
            });

        _mockEventBus
            .Setup(x => x.MarkCompletedAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(250);

        await _consumer.ExecuteAsync(cts.Token);

        Assert.True(_consumer.ProcessCallCount >= 1);
        Assert.Null(_consumer.LastProcessedEvent?.Id);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldProcessMultipleEventsSequentially()
    {
        var eventId1 = Guid.NewGuid();
        var eventId2 = Guid.NewGuid();
        var testEvent1 = new TestEvent(1, "Test1", DateTime.UtcNow);
        var testEvent2 = new TestEvent(2, "Test2", DateTime.UtcNow);
        var payload1 = Newtonsoft.Json.JsonConvert.SerializeObject(testEvent1);
        var payload2 = Newtonsoft.Json.JsonConvert.SerializeObject(testEvent2);

        var callCount = 0;
        var result1 = new ValueTuple<Guid, string?>(eventId1, payload1);
        var result2 = new ValueTuple<Guid, string?>(eventId2, payload2);
        
        _mockEventBus
            .Setup(x => x.FetchPendingAsync("Test.Event"))
            .Returns(() =>
            {
                callCount++;
                if (callCount == 1)
                    return Task.FromResult<(Guid, string?)?>result1;
                else if (callCount == 2)
                    return Task.FromResult<(Guid, string?)?>result2;
                else
                    return Task.FromResult<(Guid, string?)?>(default);
            });

        _mockEventBus
            .Setup(x => x.MarkCompletedAsync(It.IsAny<Guid>()))
            .Returns(Task.CompletedTask);

        var cts = new CancellationTokenSource();
        cts.CancelAfter(300);

        await _consumer.ExecuteAsync(cts.Token);

        Assert.True(_consumer.ProcessCallCount >= 2);
        _mockEventBus.Verify(x => x.MarkCompletedAsync(eventId1), Times.Once);
        _mockEventBus.Verify(x => x.MarkCompletedAsync(eventId2), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldRespectPollInterval()
    {
        _mockEventBus
            .Setup(x => x.FetchPendingAsync("Test.Event"))
            .ReturnsAsync(default((Guid, string?)));

        _consumer.PollIntervalMs = 100;

        var cts = new CancellationTokenSource();
        cts.CancelAfter(350);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        await _consumer.ExecuteAsync(cts.Token);
        stopwatch.Stop();

        _mockEventBus.Verify(x => x.FetchPendingAsync("Test.Event"), Times.AtLeast(3));
    }

    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        Assert.Equal("Test.Event", _consumer.EventType);
        Assert.Equal(1000, _consumer.PollIntervalMs);
        Assert.Equal(5, _consumer.MaxRetries);
    }
}
