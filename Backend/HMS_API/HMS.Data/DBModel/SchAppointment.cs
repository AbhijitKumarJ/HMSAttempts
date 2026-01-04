using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class SchAppointment
{
    public long Id { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public string? Status { get; set; }

    public string? ReasonForVisit { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ClinConsultation> ClinConsultations { get; set; } = new List<ClinConsultation>();

    public virtual User? Doctor { get; set; }

    public virtual PatPatient? Patient { get; set; }
}
