using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Billing
{
    public long BillingId { get; set; }

    public long PatientId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
