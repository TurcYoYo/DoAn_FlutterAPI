using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class Table
{
    public int TableId { get; set; }

    public string TableCode { get; set; } = null!;

    public string? TableName { get; set; }

    public int Floor { get; set; }

    public int Capacity { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<TableSession> TableSessions { get; set; } = new List<TableSession>();

    public virtual ICollection<TabletConfig> TabletConfigs { get; set; } = new List<TabletConfig>();
}
