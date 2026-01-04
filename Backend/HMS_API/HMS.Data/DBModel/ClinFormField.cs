using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class ClinFormField
{
    public int Id { get; set; }

    public int? TemplateId { get; set; }

    public int? FieldId { get; set; }

    public string? LabelOverride { get; set; }

    public int? DisplayOrder { get; set; }

    public bool? IsRequired { get; set; }

    public string? UiControl { get; set; }

    public virtual ClinFieldDefinition? Field { get; set; }

    public virtual ClinFormTemplate? Template { get; set; }
}
