using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class DailyRevenue
{
    public int RevenueId { get; set; }

    public DateOnly RevenueDate { get; set; }

    public int TotalOrders { get; set; }

    public decimal TotalAmount { get; set; }

    public DateTime UpdatedAt { get; set; }
}
