using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class BilInvoiceItem
{
    public long Id { get; set; }

    public long? InvoiceId { get; set; }

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public Guid? SourceEventId { get; set; }

    public virtual BilInvoice? Invoice { get; set; }
}
