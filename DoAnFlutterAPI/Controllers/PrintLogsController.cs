using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintLogsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public PrintLogsController(CafeDbContext context)
        {
            _context = context;
        }

        // GET: api/printlogs/order/5
        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PrintLogDto>>>> GetPrintLogsByOrder(int orderId)
        {
            var logs = await _context.PrintLogs
                .Where(pl => pl.OrderId == orderId)
                .OrderByDescending(pl => pl.PrintedAt)
                .Select(pl => new PrintLogDto
                {
                    PrintLogId = pl.PrintLogId,
                    OrderId = pl.OrderId,
                    PrinterIp = pl.PrinterIp,
                    Success = pl.Success,
                    ErrorMsg = pl.ErrorMsg,
                    PrintedAt = pl.PrintedAt
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<PrintLogDto>>
            {
                Data = logs
            });
        }

        // POST: api/printlogs
        [HttpPost]
        public async Task<ActionResult<ApiResponse<PrintLogDto>>> PostPrintLog(PrintLogDto logDto)
        {
            var log = new PrintLog
            {
                OrderId = logDto.OrderId,
                PrinterIp = logDto.PrinterIp,
                Success = logDto.Success,
                ErrorMsg = logDto.ErrorMsg,
                PrintedAt = DateTime.Now
            };

            _context.PrintLogs.Add(log);
            await _context.SaveChangesAsync();

            logDto.PrintLogId = log.PrintLogId;
            logDto.PrintedAt = log.PrintedAt;

            return Ok(new ApiResponse<PrintLogDto>
            {
                Message = "Print log recorded successfully",
                Data = logDto
            });
        }
    }
}
