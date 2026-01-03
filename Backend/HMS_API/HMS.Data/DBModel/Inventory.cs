using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public short FilmId { get; set; }

    public short StoreId { get; set; }

    public DateTime LastUpdate { get; set; }
}
