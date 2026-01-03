using System;
using System.Collections.Generic;

namespace HMS.Data.DBModel;

public partial class Payment
{
    public int PaymentId { get; set; }

    public short CustomerId { get; set; }

    public short StaffId { get; set; }

    public int RentalId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }
}
