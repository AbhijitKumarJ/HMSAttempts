using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Address
{
    public int AddressId { get; set; }

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string District { get; set; } = null!;

    public int CityId { get; set; }

    public string? PostalCode { get; set; }

    public string Phone { get; set; } = null!;

    public DateTime LastUpdate { get; set; }
}
