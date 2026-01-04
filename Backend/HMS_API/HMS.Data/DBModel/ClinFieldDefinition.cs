using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class ClinFieldDefinition
{
    public int Id { get; set; }

    public string FieldCode { get; set; } = null!;

    public string? Name { get; set; }

    public string? DataType { get; set; }

    public string? Unit { get; set; }

    public string? ValidationRules { get; set; }

    public string? Options { get; set; }

    public virtual ICollection<ClinAssessmentValue> ClinAssessmentValues { get; set; } = new List<ClinAssessmentValue>();

    public virtual ICollection<ClinFormField> ClinFormFields { get; set; } = new List<ClinFormField>();
}
