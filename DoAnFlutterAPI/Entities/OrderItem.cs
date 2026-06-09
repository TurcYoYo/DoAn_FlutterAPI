using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int OrderId { get; set; }

    public int MenuItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public string? Note { get; set; }

    public string ItemStatus { get; set; } = null!;

    public virtual ICollection<KitchenQueue> KitchenQueues { get; set; } = new List<KitchenQueue>();

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
