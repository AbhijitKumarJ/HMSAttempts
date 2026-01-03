using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Episode
{
    public long EpisodeId { get; set; }

    public long PatientId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Diagnosis { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual Patient Patient { get; set; } = null!;
}
