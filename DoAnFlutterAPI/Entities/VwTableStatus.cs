using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class VwTableStatus
{
    public int TableId { get; set; }

    public string TableCode { get; set; } = null!;

    public string? TableName { get; set; }

    public int Floor { get; set; }

    public int Capacity { get; set; }

    public string Status { get; set; } = null!;

    public int? SessionId { get; set; }

    public DateTime? SessionStart { get; set; }

    public int? TotalOrders { get; set; }

    public decimal TotalSpent { get; set; }
}
