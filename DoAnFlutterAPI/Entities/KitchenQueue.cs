using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class KitchenQueue
{
    public int QueueId { get; set; }

    public int OrderId { get; set; }

    public int OrderItemId { get; set; }

    public string TableCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public string? Note { get; set; }

    public string Status { get; set; } = null!;

    public DateTime OrderedAt { get; set; }

    public DateTime? DoneAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual OrderItem OrderItem { get; set; } = null!;
}
