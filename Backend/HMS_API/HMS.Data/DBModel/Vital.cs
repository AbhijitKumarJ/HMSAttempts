using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Vital
{
    public long VitalId { get; set; }

    public long PatientId { get; set; }

    public string? BloodPressure { get; set; }

    public int? HeartRate { get; set; }

    public double? Temperature { get; set; }

    public DateTime? RecordedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
