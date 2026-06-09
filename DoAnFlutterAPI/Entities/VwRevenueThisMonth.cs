using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class VwRevenueThisMonth
{
    public DateOnly RevenueDate { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalAmount { get; set; }

    public decimal? CumulativeAmount { get; set; }
}
