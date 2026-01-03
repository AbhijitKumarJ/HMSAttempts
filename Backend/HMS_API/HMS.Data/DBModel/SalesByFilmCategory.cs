using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class SalesByFilmCategory
{
    public string? Category { get; set; }

    public decimal? TotalSales { get; set; }
}
