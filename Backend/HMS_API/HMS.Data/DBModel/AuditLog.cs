using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class AuditLog
{
    public long Id { get; set; }

    public string? EntityType { get; set; }

    public string? EntityId { get; set; }

    public string? Action { get; set; }

    public string? OldValue { get; set; }

    public string? NewValue { get; set; }

    public string? Reason { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }
}
