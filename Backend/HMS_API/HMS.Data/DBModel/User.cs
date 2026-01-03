using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class User
{
    public long UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public long? RoleId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<AssessmentFieldValue> AssessmentFieldValues { get; set; } = new List<AssessmentFieldValue>();

    public virtual ICollection<Consultation> Consultations { get; set; } = new List<Consultation>();

    public virtual Role? Role { get; set; }
}
