using DoAnFlutterAPI.DTOs;
using DoAnFlutterAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DoAnFlutterAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TablesController : ControllerBase
{
    private readonly CafeDbContext _context;

    public TablesController(CafeDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<TableStatusDto>>>> GetTables()
    {
        var tables = await _context.VwTableStatuses.ToListAsync();
        var dtos = tables.Select(t => new TableStatusDto
        {
            TableId = t.TableId,
            TableCode = t.TableCode,
            TableName = t.TableName,
            Floor = t.Floor,
            Capacity = t.Capacity,
            Status = t.Status,
            SessionId = t.SessionId,
            SessionStart = t.SessionStart,
            TotalOrders = t.TotalOrders,
            TotalSpent = t.TotalSpent
        }).ToList();

        return Ok(new ApiResponse<IEnumerable<TableStatusDto>> { Data = dtos });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TableDto>>> GetTable(int id)
    {
        var table = await _context.Tables.FindAsync(id);

        if (table == null)
        {
            return NotFound(new ApiResponse<TableDto> { Success = false, Message = "Table not found" });
        }

        var dto = new TableDto
        {
            TableId = table.TableId,
            TableCode = table.TableCode,
            TableName = table.TableName,
            Floor = table.Floor,
            Capacity = table.Capacity,
            IsActive = table.IsActive
        };

        return Ok(new ApiResponse<TableDto> { Data = dto });
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TableDto>>> CreateTable(TableDto dto)
    {
        var table = new Table
        {
            TableCode = dto.TableCode,
            TableName = dto.TableName,
            Floor = dto.Floor,
            Capacity = dto.Capacity,
            IsActive = dto.IsActive
        };

        _context.Tables.Add(table);
        await _context.SaveChangesAsync();

        dto.TableId = table.TableId;

        return CreatedAtAction(nameof(GetTable), new { id = table.TableId }, new ApiResponse<TableDto> { Data = dto });
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TableDto>>> UpdateTable(int id, TableDto dto)
    {
        var table = await _context.Tables.FindAsync(id);
        if (table == null)
        {
            return NotFound(new ApiResponse<TableDto> { Success = false, Message = "Table not found" });
        }

        table.TableCode = dto.TableCode;
        table.TableName = dto.TableName;
        table.Floor = dto.Floor;
        table.Capacity = dto.Capacity;
        table.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        dto.TableId = table.TableId;

        return Ok(new ApiResponse<TableDto> { Data = dto });
    }
}
