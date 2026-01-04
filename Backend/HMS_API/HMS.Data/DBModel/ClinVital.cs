using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class ClinVital
{
    public long Id { get; set; }

    public int? PatientId { get; set; }

    public long? ConsultationId { get; set; }

    public int? BpSystolic { get; set; }

    public int? BpDiastolic { get; set; }

    public int? HeartRate { get; set; }

    public decimal? Temperature { get; set; }

    public int? Spo2 { get; set; }

    public DateTime? RecordedAt { get; set; }

    public int? RecordedBy { get; set; }

    public virtual ClinConsultation? Consultation { get; set; }

    public virtual PatPatient? Patient { get; set; }

    public virtual User? RecordedByNavigation { get; set; }
}
