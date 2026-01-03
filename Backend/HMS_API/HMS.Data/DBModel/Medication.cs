using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Medication
{
    public long MedicationId { get; set; }

    public long PatientId { get; set; }

    public string? MedicationName { get; set; }

    public string? Dosage { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
