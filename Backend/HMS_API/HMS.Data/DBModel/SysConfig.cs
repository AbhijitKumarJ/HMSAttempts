using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class SysConfig
{
    public string Key { get; set; } = null!;

    public string? Value { get; set; }

    public string? Description { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
