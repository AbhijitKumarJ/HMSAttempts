using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HMS.Data.DBModel;

public partial class ClinAssessmentValue
{
    public long Id { get; set; }

    public long? AssessmentId { get; set; }

    public int? FieldId { get; set; }

    public string? ValueRaw { get; set; }

    public JsonDocument? ValueTyped { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ClinAssessment? Assessment { get; set; }

    public virtual ClinFieldDefinition? Field { get; set; }
}
