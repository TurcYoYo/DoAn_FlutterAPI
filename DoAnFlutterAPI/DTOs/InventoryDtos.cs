using System;

namespace DoAnFlutterAPI.DTOs
{
    // Ingredient DTOs
    public class IngredientDto
    {
        public int IngredientId { get; set; }
        public string Name { get; set; } = null!;
        public string Unit { get; set; } = null!;
        public decimal CurrentStock { get; set; }
        public decimal MinStock { get; set; }
    }

    // Stock Transaction DTOs
    public class StockTransactionDto
    {
        public int TransactionId { get; set; }
        public int IngredientId { get; set; }
        public string TransType { get; set; } = null!;
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class StockTransactionCreateDto
    {
        public int IngredientId { get; set; }
        public string TransType { get; set; } = null!; // "in", "out"
        public decimal Amount { get; set; }
        public string? Reason { get; set; }
    }

    // Payment DTOs
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime PaidAt { get; set; }
    }

    public class PaymentCreateDto
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = null!;
        public string Status { get; set; } = "paid";
    }

    // Revenue DTOs
    public class RevenueDto
    {
        public DateOnly RevenueDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? CumulativeAmount { get; set; }
    }
}
