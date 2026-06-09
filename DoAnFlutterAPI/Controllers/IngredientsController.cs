using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public IngredientsController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<IngredientDto>>>> GetIngredients()
        {
            var ingredients = await _context.Ingredients
                .Select(i => new IngredientDto
                {
                    IngredientId = i.IngredientId,
                    Name = i.Name,
                    Unit = i.Unit,
                    CurrentStock = i.CurrentStock,
                    MinStock = i.MinStock
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<IngredientDto>>
            {
                Data = ingredients
            });
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<ApiResponse<IEnumerable<IngredientDto>>>> GetLowStockIngredients()
        {
            var lowStock = await _context.VwLowStockIngredients
                .Select(i => new IngredientDto
                {
                    IngredientId = i.IngredientId,
                    Name = i.Name,
                    Unit = i.Unit,
                    CurrentStock = i.CurrentStock,
                    MinStock = i.MinStock
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<IngredientDto>>
            {
                Data = lowStock
            });
        }
    }
}
