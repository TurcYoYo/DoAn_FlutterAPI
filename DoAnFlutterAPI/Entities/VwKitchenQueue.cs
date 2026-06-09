using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class VwKitchenQueue
{
    public int QueueId { get; set; }

    public int OrderId { get; set; }

    public string TableCode { get; set; } = null!;

    public string ItemName { get; set; } = null!;

    public int Quantity { get; set; }

    public string? Note { get; set; }

    public string Status { get; set; } = null!;

    public DateTime OrderedAt { get; set; }

    public DateTime? DoneAt { get; set; }

    public int? WaitingMinutes { get; set; }
}
