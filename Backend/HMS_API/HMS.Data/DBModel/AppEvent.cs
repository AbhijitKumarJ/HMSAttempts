using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class AppEvent
{
    public Guid Id { get; set; }

    public string EventType { get; set; } = null!;

    public string? Payload { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ProcessedAt { get; set; }

    public int? FailureCount { get; set; }
}
