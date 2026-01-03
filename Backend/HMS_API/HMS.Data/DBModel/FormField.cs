using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class FormField
{
    public long FormFieldId { get; set; }

    public long? DynamicFormId { get; set; }

    public long? FieldId { get; set; }

    public string? LabelOverride { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsRequiredOverride { get; set; }

    public string? VisibilityCondition { get; set; }

    public string? UiOptions { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AssessmentFieldValue> AssessmentFieldValues { get; set; } = new List<AssessmentFieldValue>();

    public virtual DynamicForm? DynamicForm { get; set; }

    public virtual FieldDefinition? Field { get; set; }
}
