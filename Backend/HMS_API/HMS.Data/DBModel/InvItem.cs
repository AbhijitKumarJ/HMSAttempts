using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class InvItem
{
    public string Sku { get; set; } = null!;

    public string? Name { get; set; }

    public int? Quantity { get; set; }

    public int? MinReorderLevel { get; set; }

    public decimal? UnitPrice { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<InvTransaction> InvTransactions { get; set; } = new List<InvTransaction>();
}
