using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Staff
{
    public int StaffId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public short AddressId { get; set; }

    public string? Email { get; set; }

    public short StoreId { get; set; }

    public bool Active { get; set; }

    public string Username { get; set; } = null!;

    public string? Password { get; set; }

    public DateTime LastUpdate { get; set; }

    public byte[]? Picture { get; set; }
}
