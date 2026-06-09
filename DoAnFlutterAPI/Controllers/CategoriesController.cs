using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public CategoriesController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDto>>>> GetCategories()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    IconUrl = c.IconUrl,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<CategoryDto>>
            {
                Data = categories
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Category not found"
                });
            }

            var dto = new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                IconUrl = category.IconUrl,
                IsActive = category.IsActive
            };

            return Ok(new ApiResponse<CategoryDto>
            {
                Data = dto
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> CreateCategory(CategoryCreateDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                IconUrl = dto.IconUrl,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            var resultDto = new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                IconUrl = category.IconUrl,
                IsActive = category.IsActive
            };

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, new ApiResponse<CategoryDto>
            {
                Data = resultDto
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CategoryDto>>> UpdateCategory(int id, CategoryCreateDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new ApiResponse<CategoryDto>
                {
                    Success = false,
                    Message = "Category not found"
                });
            }

            category.Name = dto.Name;
            category.IconUrl = dto.IconUrl;

            await _context.SaveChangesAsync();

            var resultDto = new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                IconUrl = category.IconUrl,
                IsActive = category.IsActive
            };

            return Ok(new ApiResponse<CategoryDto>
            {
                Data = resultDto
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Category not found"
                });
            }

            category.IsActive = false;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<string>
            {
                Message = "Category soft-deleted successfully"
            });
        }
    }
}
