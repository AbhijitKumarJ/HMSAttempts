using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Assessment
{
    public long AssessmentId { get; set; }

    public long? ConsultationId { get; set; }

    public long? DynamicFormId { get; set; }

    public DateTime? AssessmentDate { get; set; }

    public string? Summary { get; set; }

    public string? Results { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<AssessmentFieldValue> AssessmentFieldValues { get; set; } = new List<AssessmentFieldValue>();

    public virtual Consultation? Consultation { get; set; }

    public virtual DynamicForm? DynamicForm { get; set; }
}
