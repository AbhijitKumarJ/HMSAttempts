using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class ClinAssessment
{
    public long Id { get; set; }

    public int? PatientId { get; set; }

    public long? ConsultationId { get; set; }

    public int? TemplateId { get; set; }

    public DateTime? PerformedAt { get; set; }

    public int? PerformedBy { get; set; }

    public virtual ICollection<ClinAssessmentValue> ClinAssessmentValues { get; set; } = new List<ClinAssessmentValue>();

    public virtual ClinConsultation? Consultation { get; set; }

    public virtual PatPatient? Patient { get; set; }

    public virtual User? PerformedByNavigation { get; set; }

    public virtual ClinFormTemplate? Template { get; set; }
}
