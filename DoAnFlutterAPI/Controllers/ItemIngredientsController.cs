using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemIngredientsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public ItemIngredientsController(CafeDbContext context)
        {
            _context = context;
        }

        // GET: api/itemingredients/menuitem/5
        [HttpGet("menuitem/{menuItemId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ItemIngredientDto>>>> GetIngredientsByMenuItem(int menuItemId)
        {
            var ingredients = await _context.ItemIngredients
                .Where(ii => ii.MenuItemId == menuItemId)
                .Include(ii => ii.Ingredient)
                .Select(ii => new ItemIngredientDto
                {
                    ItemIngredientId = ii.ItemIngredientId,
                    MenuItemId = ii.MenuItemId,
                    IngredientId = ii.IngredientId,
                    IngredientName = ii.Ingredient.Name,
                    AmountPerServing = ii.AmountPerServing,
                    Unit = ii.Ingredient.Unit
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<ItemIngredientDto>>
            {
                Data = ingredients
            });
        }

        // POST: api/itemingredients
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ItemIngredientDto>>> PostItemIngredient(ItemIngredientCreateDto createDto)
        {
            // Check if already exists
            var existing = await _context.ItemIngredients
                .FirstOrDefaultAsync(ii => ii.MenuItemId == createDto.MenuItemId && ii.IngredientId == createDto.IngredientId);

            if (existing != null)
            {
                existing.AmountPerServing = createDto.AmountPerServing;
                await _context.SaveChangesAsync();
                
                await _context.Entry(existing).Reference(ii => ii.Ingredient).LoadAsync();

                return Ok(new ApiResponse<ItemIngredientDto>
                {
                    Message = "Item ingredient updated successfully",
                    Data = new ItemIngredientDto
                    {
                        ItemIngredientId = existing.ItemIngredientId,
                        MenuItemId = existing.MenuItemId,
                        IngredientId = existing.IngredientId,
                        IngredientName = existing.Ingredient.Name,
                        AmountPerServing = existing.AmountPerServing,
                        Unit = existing.Ingredient.Unit
                    }
                });
            }

            var itemIngredient = new ItemIngredient
            {
                MenuItemId = createDto.MenuItemId,
                IngredientId = createDto.IngredientId,
                AmountPerServing = createDto.AmountPerServing
            };

            _context.ItemIngredients.Add(itemIngredient);
            await _context.SaveChangesAsync();

            // Load navigation property
            await _context.Entry(itemIngredient).Reference(ii => ii.Ingredient).LoadAsync();

            var dto = new ItemIngredientDto
            {
                ItemIngredientId = itemIngredient.ItemIngredientId,
                MenuItemId = itemIngredient.MenuItemId,
                IngredientId = itemIngredient.IngredientId,
                IngredientName = itemIngredient.Ingredient.Name,
                AmountPerServing = itemIngredient.AmountPerServing,
                Unit = itemIngredient.Ingredient.Unit
            };

            return Ok(new ApiResponse<ItemIngredientDto>
            {
                Message = "Item ingredient added successfully",
                Data = dto
            });
        }

        // DELETE: api/itemingredients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteItemIngredient(int id)
        {
            var itemIngredient = await _context.ItemIngredients.FindAsync(id);
            if (itemIngredient == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Item ingredient not found"
                });
            }

            _context.ItemIngredients.Remove(itemIngredient);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "Item ingredient removed successfully"
            });
        }
    }
}
