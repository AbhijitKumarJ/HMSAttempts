using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class ClinMacro
{
    public int Id { get; set; }

    public string? TriggerKey { get; set; }

    public string? Expansion { get; set; }

    public int? UserId { get; set; }

    public virtual User? User { get; set; }
}
