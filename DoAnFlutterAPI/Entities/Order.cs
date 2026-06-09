using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int SessionId { get; set; }

    public int TableId { get; set; }

    public string TableCode { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public string? ZaloPayOrderId { get; set; }

    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual ICollection<KitchenQueue> KitchenQueues { get; set; } = new List<KitchenQueue>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<PrintLog> PrintLogs { get; set; } = new List<PrintLog>();

    public virtual TableSession Session { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
