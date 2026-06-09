using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DoAnFlutterAPI.Entities;
using DoAnFlutterAPI.DTOs;

namespace DoAnFlutterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TabletConfigsController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public TabletConfigsController(CafeDbContext context)
        {
            _context = context;
        }

        // GET: api/tabletconfigs
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TabletConfigDto>>>> GetTabletConfigs()
        {
            var configs = await _context.TabletConfigs
                .Include(c => c.Table)
                .Select(c => new TabletConfigDto
                {
                    ConfigId = c.ConfigId,
                    DeviceId = c.DeviceId,
                    Role = c.Role,
                    TableId = c.TableId,
                    TableCode = c.Table != null ? c.Table.TableCode : null,
                    SetupAt = c.SetupAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<TabletConfigDto>>
            {
                Data = configs
            });
        }

        // GET: api/tabletconfigs/device/{deviceId}
        [HttpGet("device/{deviceId}")]
        public async Task<ActionResult<ApiResponse<TabletConfigDto>>> GetTabletConfigByDevice(string deviceId)
        {
            var config = await _context.TabletConfigs
                .Include(c => c.Table)
                .FirstOrDefaultAsync(c => c.DeviceId == deviceId);

            if (config == null)
            {
                return NotFound(new ApiResponse<TabletConfigDto>
                {
                    Success = false,
                    Message = "Tablet configuration not found for this device"
                });
            }

            var dto = new TabletConfigDto
            {
                ConfigId = config.ConfigId,
                DeviceId = config.DeviceId,
                Role = config.Role,
                TableId = config.TableId,
                TableCode = config.Table != null ? config.Table.TableCode : null,
                SetupAt = config.SetupAt,
                UpdatedAt = config.UpdatedAt
            };

            return Ok(new ApiResponse<TabletConfigDto>
            {
                Data = dto
            });
        }

        // POST: api/tabletconfigs/setup
        [HttpPost("setup")]
        public async Task<ActionResult<ApiResponse<TabletConfigDto>>> SetupDevice(TabletConfigCreateDto setupDto)
        {
            var config = await _context.TabletConfigs
                .Include(c => c.Table)
                .FirstOrDefaultAsync(c => c.DeviceId == setupDto.DeviceId);

            if (config == null)
            {
                config = new TabletConfig
                {
                    DeviceId = setupDto.DeviceId,
                    Role = setupDto.Role,
                    TableId = setupDto.TableId,
                    SetupAt = DateTime.Now
                };
                _context.TabletConfigs.Add(config);
            }
            else
            {
                config.Role = setupDto.Role;
                config.TableId = setupDto.TableId;
                config.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            // Reload to get Table navigation property if TableId was provided
            if (config.TableId.HasValue)
            {
                await _context.Entry(config).Reference(c => c.Table).LoadAsync();
            }

            var dto = new TabletConfigDto
            {
                ConfigId = config.ConfigId,
                DeviceId = config.DeviceId,
                Role = config.Role,
                TableId = config.TableId,
                TableCode = config.Table != null ? config.Table.TableCode : null,
                SetupAt = config.SetupAt,
                UpdatedAt = config.UpdatedAt
            };

            return Ok(new ApiResponse<TabletConfigDto>
            {
                Message = "Tablet configuration setup successful",
                Data = dto
            });
        }
    }
}
