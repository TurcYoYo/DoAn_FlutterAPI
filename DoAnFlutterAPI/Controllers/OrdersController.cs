using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public OrdersController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet("session/{sessionId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<OrderResponseDto>>>> GetOrdersBySession(int sessionId)
        {
            var orders = await _context.Orders
                .Where(o => o.SessionId == sessionId)
                .Include(o => o.OrderItems)
                .Select(o => new OrderResponseDto
                {
                    OrderId = o.OrderId,
                    SessionId = o.SessionId,
                    TableId = o.TableId,
                    TableCode = o.TableCode,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    ZaloPayOrderId = o.ZaloPayOrderId, // Đã map thêm
                    Note = o.Note,
                    CreatedAt = o.CreatedAt,
                    CompletedAt = o.CompletedAt,       // Đã map thêm
                    Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        OrderItemId = oi.OrderItemId,
                        MenuItemId = oi.MenuItemId,
                        ItemName = oi.ItemName,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        Note = oi.Note,
                        ItemStatus = oi.ItemStatus
                    }).ToList()
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<OrderResponseDto>>
            {
                Data = orders
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<OrderResponseDto>>> GetOrder(int id)
        {
            var o = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (o == null)
            {
                return NotFound(new ApiResponse<OrderResponseDto>
                {
                    Success = false,
                    Message = "Order not found"
                });
            }

            var dto = new OrderResponseDto
            {
                OrderId = o.OrderId,
                SessionId = o.SessionId,
                TableId = o.TableId,
                TableCode = o.TableCode,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                ZaloPayOrderId = o.ZaloPayOrderId,
                Note = o.Note,
                CreatedAt = o.CreatedAt,
                CompletedAt = o.CompletedAt,
                Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    OrderItemId = oi.OrderItemId,
                    MenuItemId = oi.MenuItemId,
                    ItemName = oi.ItemName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    Note = oi.Note,
                    ItemStatus = oi.ItemStatus
                }).ToList()
            };

            return Ok(new ApiResponse<OrderResponseDto>
            {
                Data = dto
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<OrderResponseDto>>> PostOrder(OrderCreateDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 1. Tạo Order (Trạng thái mặc định là 'preparing')
                var order = new Order
                {
                    SessionId = dto.SessionId,
                    TableId = dto.TableId,
                    TableCode = dto.TableCode,
                    ZaloPayOrderId = dto.ZaloPayOrderId, // app_trans_id từ DTO
                    Note = dto.Note,
                    Status = "preparing", // Khớp với CK_Orders_Status
                    CreatedAt = DateTime.Now,
                    TotalAmount = 0
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                decimal totalAmount = 0;

                // 2. Tạo OrderItems, KitchenQueue và trừ tồn kho
                foreach (var itemDto in dto.Items)
                {
                    var menuItem = await _context.MenuItems
                        .Include(m => m.ItemIngredients)
                        .ThenInclude(ii => ii.Ingredient)
                        .FirstOrDefaultAsync(m => m.MenuItemId == itemDto.MenuItemId);

                    if (menuItem == null)
                    {
                        throw new Exception($"Menu item {itemDto.MenuItemId} not found");
                    }

                    // Trừ StockQty của MenuItem (nếu có quản lý theo số lượng món)
                    if (menuItem.StockQty > 0)
                    {
                        menuItem.StockQty -= itemDto.Quantity;
                        if (menuItem.StockQty <= 0)
                        {
                            menuItem.StockQty = 0;
                            menuItem.IsAvailable = false;
                        }
                    }

                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        MenuItemId = menuItem.MenuItemId,
                        ItemName = menuItem.Name,
                        Quantity = itemDto.Quantity,
                        UnitPrice = menuItem.Price,
                        Note = itemDto.Note,
                        ItemStatus = "pending"
                    };

                    _context.OrderItems.Add(orderItem);
                    await _context.SaveChangesAsync(); // Lưu để có OrderItemId cho StockTransaction

                    // Trừ tồn kho nguyên liệu (Ingredients) dựa trên công thức (ItemIngredients)
                    foreach (var recipe in menuItem.ItemIngredients)
                    {
                        var ingredient = recipe.Ingredient;
                        decimal totalNeeded = recipe.AmountPerServing * itemDto.Quantity;

                        if (ingredient.CurrentStock < totalNeeded)
                        {
                            throw new Exception($"Không đủ nguyên liệu: {ingredient.Name} (Cần {totalNeeded}{ingredient.Unit}, hiện có {ingredient.CurrentStock}{ingredient.Unit})");
                        }

                        ingredient.CurrentStock -= totalNeeded;
                        ingredient.UpdatedAt = DateTime.Now;

                        // Ghi log giao dịch kho
                        var stockTrans = new StockTransaction
                        {
                            IngredientId = ingredient.IngredientId,
                            TransType = "out",
                            Amount = totalNeeded,
                            Reason = $"Order #{order.OrderId} - {menuItem.Name}",
                            OrderItemId = orderItem.OrderItemId,
                            CreatedAt = DateTime.Now
                        };
                        _context.StockTransactions.Add(stockTrans);
                    }

                    var kitchenQueue = new KitchenQueue
                    {
                        OrderId = order.OrderId,
                        OrderItemId = orderItem.OrderItemId,
                        TableCode = dto.TableCode,
                        ItemName = menuItem.Name,
                        Quantity = itemDto.Quantity,
                        Note = itemDto.Note,
                        Status = "waiting",
                        OrderedAt = DateTime.Now
                    };

                    _context.KitchenQueues.Add(kitchenQueue);
                    totalAmount += orderItem.UnitPrice * orderItem.Quantity;
                }

                // Cập nhật lại tổng tiền cho Order
                order.TotalAmount = totalAmount;
                await _context.SaveChangesAsync();

                // 3. TẠO BẢN GHI PAYMENT (Bắt buộc theo rule "Chỉ Insert khi ZaloPay thành công")
                var payment = new Payment
                {
                    OrderId = order.OrderId,
                    ZaloPayOrderId = dto.ZaloPayOrderId,
                    ZaloTransId = dto.ZaloTransId, // Lấy từ DTO do webhook/app truyền lên
                    Amount = totalAmount,
                    Method = "zalopay",
                    Status = "success", // Khớp với CK_Payments_Status
                    PaidAt = DateTime.Now
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                // Hoàn tất Transaction
                await transaction.CommitAsync();

                // Reload for response
                return await GetOrder(order.OrderId);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new ApiResponse<OrderResponseDto>
                {
                    Success = false,
                    Message = ex.InnerException != null ? ex.InnerException.Message : ex.Message
                });
            }
        }
    }
}