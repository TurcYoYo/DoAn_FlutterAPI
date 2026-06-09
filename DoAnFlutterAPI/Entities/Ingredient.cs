using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal CurrentStock { get; set; }

    public decimal MinStock { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ItemIngredient> ItemIngredients { get; set; } = new List<ItemIngredient>();

    public virtual ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
}
