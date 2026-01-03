using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class AssessmentFieldValue
{
    public long AssessmentFieldValueId { get; set; }

    public long AssessmentId { get; set; }

    public long? FieldId { get; set; }

    public long? FormFieldId { get; set; }

    public string? RawValue { get; set; }

    public string? TypedValue { get; set; }

    public string? Unit { get; set; }

    public long? RecordedByUserId { get; set; }

    public DateTime? RecordedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Assessment Assessment { get; set; } = null!;

    public virtual FieldDefinition? Field { get; set; }

    public virtual FormField? FormField { get; set; }

    public virtual User? RecordedByUser { get; set; }
}
