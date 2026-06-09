using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public MenuItemsController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuItemDto>>>> GetMenuItems()
        {
            Console.WriteLine("--- API: GetMenuItems called ---");

            var allItemsCount = await _context.MenuItems.CountAsync();
            Console.WriteLine($"Total items in DB: {allItemsCount}");

            var items = await _context.MenuItems
                .Where(m => m.IsAvailable) // Chỉ lấy các món đang kinh doanh (chưa bị xóa)
                .Include(m => m.Category)
                .Select(m => new MenuItemDto
                {
                    MenuItemId = m.MenuItemId,
                    CategoryId = m.CategoryId,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageUrl = m.ImageUrl,
                    IsAvailable = m.IsAvailable,
                    StockQty = m.StockQty,
                    SortOrder = m.SortOrder,
                    CategoryName = m.Category.Name
                })
                .ToListAsync();

            Console.WriteLine($"Items returned after filter (IsAvailable=true): {items.Count}");
            foreach (var item in items)
            {
                Console.WriteLine($"- ID: {item.MenuItemId}, Name: {item.Name}, Available: {item.IsAvailable}");
            }
            Console.WriteLine("-------------------------------");

            return Ok(new ApiResponse<IEnumerable<MenuItemDto>>
            {
                Data = items
            });
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<MenuItemDto>>>> GetMenuItemsByCategory(int categoryId)
        {
            var items = await _context.MenuItems
                .Where(m => m.CategoryId == categoryId && m.IsAvailable) // Lọc món đã xóa
                .Include(m => m.Category)
                .Select(m => new MenuItemDto
                {
                    MenuItemId = m.MenuItemId,
                    CategoryId = m.CategoryId,
                    Name = m.Name,
                    Description = m.Description,
                    Price = m.Price,
                    ImageUrl = m.ImageUrl,
                    IsAvailable = m.IsAvailable,
                    StockQty = m.StockQty,
                    SortOrder = m.SortOrder,
                    CategoryName = m.Category.Name
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<MenuItemDto>>
            {
                Data = items
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> GetMenuItem(int id)
        {
            var m = await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.MenuItemId == id && m.IsAvailable); // Không tìm món đã xóa

            if (m == null)
            {
                return NotFound(new ApiResponse<MenuItemDto>
                {
                    Success = false,
                    Message = "Menu item not found or has been deleted"
                });
            }

            var dto = new MenuItemDto
            {
                MenuItemId = m.MenuItemId,
                CategoryId = m.CategoryId,
                Name = m.Name,
                Description = m.Description,
                Price = m.Price,
                ImageUrl = m.ImageUrl,
                IsAvailable = m.IsAvailable,
                StockQty = m.StockQty,
                SortOrder = m.SortOrder,
                CategoryName = m.Category.Name
            };

            return Ok(new ApiResponse<MenuItemDto>
            {
                Data = dto
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> CreateMenuItem(MenuItemCreateDto dto)
        {
            var item = new MenuItem
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl,
                IsAvailable = true, // Mặc định là true khi tạo mới
                StockQty = dto.StockQty,
                SortOrder = dto.SortOrder,
                CreatedAt = DateTime.Now
            };

            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();

            // Reload to get category name
            await _context.Entry(item).Reference(i => i.Category).LoadAsync();

            var resultDto = new MenuItemDto
            {
                MenuItemId = item.MenuItemId,
                CategoryId = item.CategoryId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                IsAvailable = item.IsAvailable,
                StockQty = item.StockQty,
                SortOrder = item.SortOrder,
                CategoryName = item.Category?.Name
            };

            return CreatedAtAction(nameof(GetMenuItem), new { id = item.MenuItemId }, new ApiResponse<MenuItemDto>
            {
                Data = resultDto
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<MenuItemDto>>> UpdateMenuItem(int id, MenuItemCreateDto dto)
        {
            var item = await _context.MenuItems.FindAsync(id);

            if (item == null || !item.IsAvailable)
            {
                return NotFound(new ApiResponse<MenuItemDto>
                {
                    Success = false,
                    Message = "Menu item not found"
                });
            }

            item.CategoryId = dto.CategoryId;
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.ImageUrl = dto.ImageUrl;
            item.IsAvailable = dto.IsAvailable;
            item.StockQty = dto.StockQty;
            item.SortOrder = dto.SortOrder;
            item.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
            await _context.Entry(item).Reference(i => i.Category).LoadAsync();

            var resultDto = new MenuItemDto
            {
                MenuItemId = item.MenuItemId,
                CategoryId = item.CategoryId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                IsAvailable = item.IsAvailable,
                StockQty = item.StockQty,
                SortOrder = item.SortOrder,
                CategoryName = item.Category?.Name
            };

            return Ok(new ApiResponse<MenuItemDto>
            {
                Data = resultDto
            });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteMenuItem(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);

            if (item == null)
            {
                return NotFound(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Menu item not found"
                });
            }

            // Thực hiện Soft Delete: Đánh dấu món là không khả dụng thay vì xóa thực sự
            // Điều này giúp tránh lỗi ràng buộc khóa ngoại (Foreign Key) với bảng OrderItems
            item.IsAvailable = false;
            item.UpdatedAt = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Menu item has been marked as unavailable (soft deleted)"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error updating database: {ex.Message}"
                });
            }
        }
    }
}
