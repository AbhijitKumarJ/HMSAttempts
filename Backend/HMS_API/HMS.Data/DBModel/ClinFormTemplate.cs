using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class ClinFormTemplate
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public bool? IsActive { get; set; }

    public int? Version { get; set; }

    public virtual ICollection<ClinAssessment> ClinAssessments { get; set; } = new List<ClinAssessment>();

    public virtual ICollection<ClinFormField> ClinFormFields { get; set; } = new List<ClinFormField>();
}
