using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class VwMenuWithStock
{
    public int MenuItemId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public int StockQty { get; set; }

    public bool IsAvailable { get; set; }

    public string CategoryName { get; set; } = null!;

    public decimal MaxServableByIngredients { get; set; }
}
