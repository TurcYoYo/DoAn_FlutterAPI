using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public ReportsController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet("revenue/daily")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RevenueDto>>>> GetDailyRevenue()
        {
            var daily = await _context.DailyRevenues
                .OrderByDescending(r => r.RevenueDate)
                .Select(r => new RevenueDto
                {
                    RevenueDate = r.RevenueDate,
                    TotalOrders = r.TotalOrders,
                    TotalAmount = r.TotalAmount
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<RevenueDto>>
            {
                Data = daily
            });
        }

        [HttpGet("revenue/monthly")]
        public async Task<ActionResult<ApiResponse<IEnumerable<RevenueDto>>>> GetMonthlyRevenue()
        {
            var monthly = await _context.VwRevenueThisMonths
                .OrderBy(r => r.RevenueDate)
                .Select(r => new RevenueDto
                {
                    RevenueDate = r.RevenueDate,
                    TotalOrders = r.TotalOrders,
                    TotalAmount = r.TotalAmount,
                    CumulativeAmount = r.CumulativeAmount
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<RevenueDto>>
            {
                Data = monthly
            });
        }

        [HttpGet("summary")]
        public async Task<ActionResult<ApiResponse<ReportSummaryDto>>> GetReportSummary([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            // Set end of day for 'to' date
            var endTo = to.Date.AddDays(1).AddTicks(-1);
            var startFrom = from.Date;

            var totalRevenue = await _context.Orders
                .Where(o => o.CreatedAt >= startFrom && o.CreatedAt <= endTo && o.Status == "done")
                .SumAsync(o => o.TotalAmount);

            var totalOrders = await _context.Orders
                .Where(o => o.CreatedAt >= startFrom && o.CreatedAt <= endTo && o.Status == "done")
                .CountAsync();

            var totalIngredients = await _context.Ingredients.CountAsync();
            var lowStockCount = await _context.Ingredients.CountAsync(i => i.CurrentStock <= i.MinStock);

            return Ok(new ApiResponse<ReportSummaryDto>
            {
                Data = new ReportSummaryDto
                {
                    TotalRevenue = totalRevenue,
                    TotalOrders = totalOrders,
                    TotalIngredients = totalIngredients,
                    LowStockCount = lowStockCount
                }
            });
        }
    }
}
