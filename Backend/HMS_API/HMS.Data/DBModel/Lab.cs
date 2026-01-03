using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Lab
{
    public long LabId { get; set; }

    public string LabName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
