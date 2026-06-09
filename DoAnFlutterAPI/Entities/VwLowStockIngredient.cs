using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class VwLowStockIngredient
{
    public int IngredientId { get; set; }

    public string Name { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal CurrentStock { get; set; }

    public decimal MinStock { get; set; }

    public decimal? Deficit { get; set; }

    public string StockStatus { get; set; } = null!;
}
