using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class MenuItem
{
    public int MenuItemId { get; set; }

    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsAvailable { get; set; }

    public int StockQty { get; set; }

    public int SortOrder { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<ItemIngredient> ItemIngredients { get; set; } = new List<ItemIngredient>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
