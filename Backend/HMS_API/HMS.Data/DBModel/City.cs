using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class City
{
    public int CityId { get; set; }

    public string City1 { get; set; } = null!;

    public int CountryId { get; set; }

    public DateTime LastUpdate { get; set; }
}
