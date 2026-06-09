using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class TableSession
{
    public int SessionId { get; set; }

    public int TableId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime OpenedAt { get; set; }

    public DateTime? ClosedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Table Table { get; set; } = null!;
}
