using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KitchenController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public KitchenController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet("queue")]
        public async Task<ActionResult<ApiResponse<IEnumerable<KitchenQueueDto>>>> GetQueue()
        {
            var queue = await _context.VwKitchenQueues
                .Where(q => q.Status != "done")
                .OrderBy(q => q.OrderedAt)
                .Select(q => new KitchenQueueDto
                {
                    QueueId = q.QueueId,
                    OrderId = q.OrderId,
                    TableCode = q.TableCode,
                    ItemName = q.ItemName,
                    Quantity = q.Quantity,
                    Note = q.Note,
                    Status = q.Status,
                    OrderedAt = q.OrderedAt,
                    DoneAt = q.DoneAt,
                    WaitingMinutes = q.WaitingMinutes
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<KitchenQueueDto>>
            {
                Data = queue
            });
        }

        [HttpPut("status/{queueId}")]
        public async Task<ActionResult<ApiResponse<string>>> UpdateStatus(int queueId, [FromBody] string status)
        {
            var item = await _context.KitchenQueues
                .Include(q => q.OrderItem)
                .FirstOrDefaultAsync(q => q.QueueId == queueId);

            if (item == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Kitchen queue item not found"
                });
            }

            item.Status = status;
            if (status == "done")
            {
                item.DoneAt = DateTime.Now;
            }

            // Sync with OrderItem
            if (item.OrderItem != null)
            {
                item.OrderItem.ItemStatus = status;
            }

            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>
            {
                Message = $"Status updated to {status}"
            });
        }
    }
}
