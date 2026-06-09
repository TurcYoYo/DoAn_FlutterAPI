using System;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Entities;

public partial class StockTransaction
{
    public int TransactionId { get; set; }

    public int IngredientId { get; set; }

    public string TransType { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Reason { get; set; }

    public int? OrderItemId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;
}
