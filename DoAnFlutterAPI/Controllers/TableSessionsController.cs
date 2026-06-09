using DoAnFlutterAPI.DTOs;
using DoAnFlutterAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DoAnFlutterAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TableSessionsController : ControllerBase
{
    private readonly CafeDbContext _context;

    public TableSessionsController(CafeDbContext context)
    {
        _context = context;
    }

    [HttpPost("open")]
    public async Task<ActionResult<ApiResponse<SessionDto>>> OpenSession(SessionCreateDto dto)
    {
        var existingSession = await _context.TableSessions
            .FirstOrDefaultAsync(s => s.TableId == dto.TableId && s.Status == "open");

        if (existingSession != null)
        {
            return BadRequest(new ApiResponse<SessionDto> { Success = false, Message = "Table already has an open session." });
        }

        var newSession = new TableSession
        {
            TableId = dto.TableId,
            Status = "open",
            OpenedAt = DateTime.UtcNow
        };

        _context.TableSessions.Add(newSession);
        await _context.SaveChangesAsync();

        var resultDto = new SessionDto
        {
            SessionId = newSession.SessionId,
            TableId = newSession.TableId,
            Status = newSession.Status,
            OpenedAt = newSession.OpenedAt,
            ClosedAt = newSession.ClosedAt
        };

        return Ok(new ApiResponse<SessionDto> { Data = resultDto });
    }

    [HttpPost("close/{id}")]
    public async Task<ActionResult<ApiResponse<SessionDto>>> CloseSession(int id)
    {
        var session = await _context.TableSessions.FindAsync(id);

        if (session == null)
        {
            return NotFound(new ApiResponse<SessionDto> { Success = false, Message = "Session not found." });
        }

        if (session.Status == "closed")
        {
            return BadRequest(new ApiResponse<SessionDto> { Success = false, Message = "Session is already closed." });
        }

        session.Status = "closed";
        session.ClosedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var resultDto = new SessionDto
        {
            SessionId = session.SessionId,
            TableId = session.TableId,
            Status = session.Status,
            OpenedAt = session.OpenedAt,
            ClosedAt = session.ClosedAt
        };

        return Ok(new ApiResponse<SessionDto> { Data = resultDto });
    }
}
