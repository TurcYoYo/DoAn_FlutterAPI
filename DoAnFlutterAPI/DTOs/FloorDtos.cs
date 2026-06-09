using System;

namespace DoAnFlutterAPI.DTOs;

public class TableDto
{
    public int TableId { get; set; }
    public string TableCode { get; set; } = null!;
    public string? TableName { get; set; }
    public int Floor { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
}

public class TableStatusDto
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

public class SessionDto
{
    public int SessionId { get; set; }
    public int TableId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime OpenedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}

public class SessionCreateDto
{
    public int TableId { get; set; }
}

public class TabletConfigDto
{
    public int ConfigId { get; set; }
    public string DeviceId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int? TableId { get; set; }
    public string? TableCode { get; set; }
    public DateTime SetupAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class TabletConfigCreateDto
{
    public string DeviceId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public int? TableId { get; set; }
}
