using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Rental
{
    public int RentalId { get; set; }

    public DateTime RentalDate { get; set; }

    public int InventoryId { get; set; }

    public short CustomerId { get; set; }

    public DateTime? ReturnDate { get; set; }

    public short StaffId { get; set; }

    public DateTime LastUpdate { get; set; }
}
