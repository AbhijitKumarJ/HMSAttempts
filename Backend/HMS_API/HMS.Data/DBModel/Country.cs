using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Country
{
    public int CountryId { get; set; }

    public string Country1 { get; set; } = null!;

    public DateTime LastUpdate { get; set; }
}
