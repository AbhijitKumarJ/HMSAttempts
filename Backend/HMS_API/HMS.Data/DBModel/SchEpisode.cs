using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class SchEpisode
{
    public long Id { get; set; }

    public int? PatientId { get; set; }

    public string? Title { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<ClinConsultation> ClinConsultations { get; set; } = new List<ClinConsultation>();

    public virtual PatPatient? Patient { get; set; }
}
