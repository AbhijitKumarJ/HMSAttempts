using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class LabResult
{
    public long Id { get; set; }

    public long? OrderId { get; set; }

    public string? ResultSummary { get; set; }

    public string? ResultData { get; set; }

    public DateTime? ReleasedAt { get; set; }

    public virtual OrdOrder? Order { get; set; }
}
