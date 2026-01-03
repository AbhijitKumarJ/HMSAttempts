using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Appointment
{
    public long AppointmentId { get; set; }

    public long PatientId { get; set; }

    public long? DoctorId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User? Doctor { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
