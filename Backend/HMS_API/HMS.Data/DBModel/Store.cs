using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Store
{
    public int StoreId { get; set; }

    public int ManagerStaffId { get; set; }

    public int AddressId { get; set; }

    public DateTime LastUpdate { get; set; }
}
