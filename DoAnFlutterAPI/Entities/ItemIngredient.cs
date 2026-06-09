using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class ItemIngredient
{
    public int ItemIngredientId { get; set; }

    public int MenuItemId { get; set; }

    public int IngredientId { get; set; }

    public decimal AmountPerServing { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual MenuItem MenuItem { get; set; } = null!;
}
