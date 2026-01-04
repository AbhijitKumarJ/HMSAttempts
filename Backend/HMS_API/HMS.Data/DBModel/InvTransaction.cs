using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class InvTransaction
{
    public long Id { get; set; }

    public string? Sku { get; set; }

    public int? ChangeAmount { get; set; }

    public string? Reason { get; set; }

    public int? UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual InvItem? SkuNavigation { get; set; }

    public virtual User? User { get; set; }
}
