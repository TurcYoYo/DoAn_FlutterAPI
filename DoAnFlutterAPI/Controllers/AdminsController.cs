using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public AdminsController(CafeDbContext context)
        {
            _context = context;
        }

        // GET: api/admins
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AdminDto>>>> GetAdmins()
        {
            var admins = await _context.Admins
                .Select(a => new AdminDto
                {
                    AdminId = a.AdminId,
                    Username = a.Username,
                    FullName = a.FullName,
                    IsActive = a.IsActive,
                    CreatedAt = a.CreatedAt,
                    LastLoginAt = a.LastLoginAt
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<AdminDto>>
            {
                Data = admins
            });
        }

        // GET: api/admins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AdminDto>>> GetAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);

            if (admin == null)
            {
                return NotFound(new ApiResponse<AdminDto>
                {
                    Success = false,
                    Message = "Admin not found"
                });
            }

            var adminDto = new AdminDto
            {
                AdminId = admin.AdminId,
                Username = admin.Username,
                FullName = admin.FullName,
                IsActive = admin.IsActive,
                CreatedAt = admin.CreatedAt,
                LastLoginAt = admin.LastLoginAt
            };

            return Ok(new ApiResponse<AdminDto>
            {
                Data = adminDto
            });
        }

        // POST: api/admins/login
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AdminDto>>> Login(AdminLoginDto loginDto)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Username == loginDto.Username && a.PasswordHash == loginDto.Password);

            if (admin == null)
            {
                return Unauthorized(new ApiResponse<AdminDto>
                {
                    Success = false,
                    Message = "Invalid username or password"
                });
            }

            if (!admin.IsActive)
            {
                return BadRequest(new ApiResponse<AdminDto>
                {
                    Success = false,
                    Message = "Account is disabled"
                });
            }

            admin.LastLoginAt = DateTime.Now;
            await _context.SaveChangesAsync();

            var adminDto = new AdminDto
            {
                AdminId = admin.AdminId,
                Username = admin.Username,
                FullName = admin.FullName,
                IsActive = admin.IsActive,
                CreatedAt = admin.CreatedAt,
                LastLoginAt = admin.LastLoginAt
            };

            return Ok(new ApiResponse<AdminDto>
            {
                Message = "Login successful",
                Data = adminDto
            });
        }

        // POST: api/admins
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AdminDto>>> PostAdmin(AdminCreateDto createDto)
        {
            if (await _context.Admins.AnyAsync(a => a.Username == createDto.Username))
            {
                return BadRequest(new ApiResponse<AdminDto>
                {
                    Success = false,
                    Message = "Username already exists"
                });
            }

            var admin = new Admin
            {
                Username = createDto.Username,
                PasswordHash = createDto.Password, // Direct string comparison as requested
                FullName = createDto.FullName,
                IsActive = createDto.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            var adminDto = new AdminDto
            {
                AdminId = admin.AdminId,
                Username = admin.Username,
                FullName = admin.FullName,
                IsActive = admin.IsActive,
                CreatedAt = admin.CreatedAt
            };

            return CreatedAtAction(nameof(GetAdmin), new { id = admin.AdminId }, new ApiResponse<AdminDto>
            {
                Message = "Admin created successfully",
                Data = adminDto
            });
        }

        // PUT: api/admins/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AdminDto>>> PutAdmin(int id, AdminUpdateDto updateDto)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound(new ApiResponse<AdminDto>
                {
                    Success = false,
                    Message = "Admin not found"
                });
            }

            if (updateDto.FullName != null) admin.FullName = updateDto.FullName;
            if (updateDto.IsActive.HasValue) admin.IsActive = updateDto.IsActive.Value;
            if (!string.IsNullOrEmpty(updateDto.Password)) admin.PasswordHash = updateDto.Password;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdminExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var adminDto = new AdminDto
            {
                AdminId = admin.AdminId,
                Username = admin.Username,
                FullName = admin.FullName,
                IsActive = admin.IsActive,
                CreatedAt = admin.CreatedAt,
                LastLoginAt = admin.LastLoginAt
            };

            return Ok(new ApiResponse<AdminDto>
            {
                Message = "Admin updated successfully",
                Data = adminDto
            });
        }

        // DELETE: api/admins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Admin not found"
                });
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "Admin deleted successfully"
            });
        }

        private bool AdminExists(int id)
        {
            return _context.Admins.Any(e => e.AdminId == id);
        }
    }
}
