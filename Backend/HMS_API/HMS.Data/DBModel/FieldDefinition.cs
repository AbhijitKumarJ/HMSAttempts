using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class FieldDefinition
{
    public long FieldId { get; set; }

    public string FieldCode { get; set; } = null!;

    public string? FieldName { get; set; }

    public string? Unit { get; set; }

    public string? AllowedValues { get; set; }

    public string? ValidationRules { get; set; }

    public bool? IsRepeatable { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AssessmentFieldValue> AssessmentFieldValues { get; set; } = new List<AssessmentFieldValue>();

    public virtual ICollection<FormField> FormFields { get; set; } = new List<FormField>();
}
