using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class OrdOrder
{
    public long Id { get; set; }

    public int? PatientId { get; set; }

    public long? ConsultationId { get; set; }

    public string? Type { get; set; }

    public string? Description { get; set; }

    public string? Priority { get; set; }

    public string? Status { get; set; }

    public int? OrderedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ClinConsultation? Consultation { get; set; }

    public virtual ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();

    public virtual User? OrderedByNavigation { get; set; }

    public virtual PatPatient? Patient { get; set; }
}
