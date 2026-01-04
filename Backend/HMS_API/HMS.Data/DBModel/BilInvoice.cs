using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class BilInvoice
{
    public long Id { get; set; }

    public int? PatientId { get; set; }

    public decimal? TotalAmount { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<BilInvoiceItem> BilInvoiceItems { get; set; } = new List<BilInvoiceItem>();

    public virtual PatPatient? Patient { get; set; }
}
