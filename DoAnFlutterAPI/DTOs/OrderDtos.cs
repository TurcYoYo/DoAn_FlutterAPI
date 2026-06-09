namespace DoAnFlutterAPI.DTOs
{
    public class OrderItemCreateDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }

    public class OrderCreateDto
    {
        public int SessionId { get; set; }
        public int TableId { get; set; }
        public string TableCode { get; set; } = null!;
        public string? Note { get; set; }

        // --- BỔ SUNG 2 TRƯỜNG NÀY ĐỂ FIX LỖI CS1061 ---
        public string? ZaloPayOrderId { get; set; }
        public string? ZaloTransId { get; set; }
        // ----------------------------------------------

        public List<OrderItemCreateDto> Items { get; set; } = new();
    }

    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public int SessionId { get; set; }
        public int TableId { get; set; }
        public string TableCode { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }

        // --- BỔ SUNG 2 TRƯỜNG NÀY ĐỂ FIX LỖI CS0117 ---
        public string? ZaloPayOrderId { get; set; }
        public DateTime? CompletedAt { get; set; }
        // ----------------------------------------------

        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class OrderItemResponseDto
    {
        public int OrderItemId { get; set; }
        public int MenuItemId { get; set; }
        public string ItemName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Note { get; set; }
        public string ItemStatus { get; set; } = null!;
    }

    public class KitchenQueueDto
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

    public class PrintLogDto
    {
        public int PrintLogId { get; set; }
        public int OrderId { get; set; }
        public string? PrinterIp { get; set; }
        public bool Success { get; set; }
        public string? ErrorMsg { get; set; }
        public DateTime PrintedAt { get; set; }
    }
}