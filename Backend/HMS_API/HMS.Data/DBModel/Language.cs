using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Language
{
    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime LastUpdate { get; set; }
}
