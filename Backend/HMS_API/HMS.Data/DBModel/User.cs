using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<ClinAssessment> ClinAssessments { get; set; } = new List<ClinAssessment>();

    public virtual ICollection<ClinConsultation> ClinConsultations { get; set; } = new List<ClinConsultation>();

    public virtual ICollection<ClinMacro> ClinMacros { get; set; } = new List<ClinMacro>();

    public virtual ICollection<ClinVital> ClinVitals { get; set; } = new List<ClinVital>();

    public virtual ICollection<InvTransaction> InvTransactions { get; set; } = new List<InvTransaction>();

    public virtual ICollection<OrdOrder> OrdOrders { get; set; } = new List<OrdOrder>();

    public virtual ICollection<SchAppointment> SchAppointments { get; set; } = new List<SchAppointment>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
     
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
