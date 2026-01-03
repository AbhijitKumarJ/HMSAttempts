using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Order
{
    public long OrderId { get; set; }

    public long PatientId { get; set; }

    public long? LabId { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Lab? Lab { get; set; }

    public virtual Patient Patient { get; set; } = null!;
}
