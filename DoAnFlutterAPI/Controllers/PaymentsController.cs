using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public PaymentsController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PaymentDto>>> CreatePayment(PaymentCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var order = await _context.Orders.FindAsync(dto.OrderId);
                if (order == null)
                {
                    return NotFound(new ApiResponse<PaymentDto>
                    {
                        Success = false,
                        Message = "Order not found"
                    });
                }

                var payment = new Payment
                {
                    OrderId = dto.OrderId,
                    Amount = dto.Amount,
                    Method = dto.Method,
                    Status = dto.Status,
                    PaidAt = DateTime.UtcNow
                };

                order.Status = "paid";

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var resultDto = new PaymentDto
                {
                    PaymentId = payment.PaymentId,
                    OrderId = payment.OrderId,
                    Amount = payment.Amount,
                    Method = payment.Method,
                    Status = payment.Status,
                    PaidAt = payment.PaidAt
                };

                return Ok(new ApiResponse<PaymentDto>
                {
                    Data = resultDto,
                    Message = "Payment recorded and order updated to paid"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new ApiResponse<PaymentDto>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }
    }
}
