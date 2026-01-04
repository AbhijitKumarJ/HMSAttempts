using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class PatPatient
{
    public int Id { get; set; }

    public string Mrn { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Dob { get; set; }

    public string? ContactInfo { get; set; }

    public bool? IsEmergencyReg { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BilInvoice> BilInvoices { get; set; } = new List<BilInvoice>();

    public virtual ICollection<ClinAssessment> ClinAssessments { get; set; } = new List<ClinAssessment>();

    public virtual ICollection<ClinConsultation> ClinConsultations { get; set; } = new List<ClinConsultation>();

    public virtual ICollection<ClinVital> ClinVitals { get; set; } = new List<ClinVital>();

    public virtual ICollection<OrdOrder> OrdOrders { get; set; } = new List<OrdOrder>();

    public virtual ICollection<SchAppointment> SchAppointments { get; set; } = new List<SchAppointment>();

    public virtual ICollection<SchEpisode> SchEpisodes { get; set; } = new List<SchEpisode>();
}
