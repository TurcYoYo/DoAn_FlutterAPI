using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockTransactionsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public StockTransactionsController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpPost("transaction")]
        public async Task<ActionResult<ApiResponse<StockTransactionDto>>> CreateTransaction(StockTransactionCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var ingredient = await _context.Ingredients.FindAsync(dto.IngredientId);
                if (ingredient == null)
                {
                    return NotFound(new ApiResponse<StockTransactionDto>
                    {
                        Success = false,
                        Message = "Ingredient not found"
                    });
                }

                var stockTransaction = new StockTransaction
                {
                    IngredientId = dto.IngredientId,
                    TransType = dto.TransType.ToLower(),
                    Amount = dto.Amount,
                    Reason = dto.Reason,
                    CreatedAt = DateTime.UtcNow
                };

                if (stockTransaction.TransType == "in")
                {
                    ingredient.CurrentStock += dto.Amount;
                }
                else if (stockTransaction.TransType == "out")
                {
                    ingredient.CurrentStock -= dto.Amount;
                }
                else
                {
                    return BadRequest(new ApiResponse<StockTransactionDto>
                    {
                        Success = false,
                        Message = "Invalid transaction type. Use 'in' or 'out'."
                    });
                }

                _context.StockTransactions.Add(stockTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                var resultDto = new StockTransactionDto
                {
                    TransactionId = stockTransaction.TransactionId,
                    IngredientId = stockTransaction.IngredientId,
                    TransType = stockTransaction.TransType,
                    Amount = stockTransaction.Amount,
                    Reason = stockTransaction.Reason,
                    CreatedAt = stockTransaction.CreatedAt
                };

                return Ok(new ApiResponse<StockTransactionDto>
                {
                    Data = resultDto,
                    Message = "Stock updated successfully"
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new ApiResponse<StockTransactionDto>
                {
                    Success = false,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }
    }
}
