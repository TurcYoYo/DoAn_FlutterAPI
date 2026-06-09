using System;

namespace DoAnFlutterAPI.DTOs
{
    public class ReportSummaryDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalIngredients { get; set; }
        public decimal LowStockCount { get; set; }
    }
}
