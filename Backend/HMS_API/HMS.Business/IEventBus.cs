using System.Text.Json.Serialization;

namespace HMS.Bus;

public record EventPayload(Guid Id, string EventType, object Data);

public interface IEventBus
{
    Task<Guid> PublishAsync(string eventType, object payload);
    Task<(Guid Id, string? Payload)?> FetchPendingAsync(string eventType);
    Task MarkCompletedAsync(Guid id);
    Task MarkFailedAsync(Guid id);
}
