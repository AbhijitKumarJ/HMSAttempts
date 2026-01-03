using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Consultation
{
    public long ConsultationId { get; set; }

    public long PatientId { get; set; }

    public long? DoctorId { get; set; }

    public long? EpisodeId { get; set; }

    public DateTime? EncounterDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();

    public virtual User? Doctor { get; set; }

    public virtual Episode? Episode { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
