using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class ClinConsultation
{
    public long Id { get; set; }

    public long? AppointmentId { get; set; }

    public long? EpisodeId { get; set; }

    public int? PatientId { get; set; }

    public int? DoctorId { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public string? ClinicalSummary { get; set; }

    public virtual SchAppointment? Appointment { get; set; }

    public virtual ICollection<ClinAssessment> ClinAssessments { get; set; } = new List<ClinAssessment>();

    public virtual ICollection<ClinVital> ClinVitals { get; set; } = new List<ClinVital>();

    public virtual User? Doctor { get; set; }

    public virtual SchEpisode? Episode { get; set; }

    public virtual ICollection<OrdOrder> OrdOrders { get; set; } = new List<OrdOrder>();

    public virtual PatPatient? Patient { get; set; }
}
