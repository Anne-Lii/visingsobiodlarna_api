using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //Kräver att användaren är inloggad
public class WinteringController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public WinteringController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Skapar en ny invintringsrapport
    [HttpPost]
    public async Task<IActionResult> CreateWintering(WinteringModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

            //Sätter UserId från inloggad användare
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            
            if (userId == null)
                return Unauthorized();

            model.UserId = userId;

        _context.Winterings.Add(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }

    //Hämtar alla invintringsrapporter för en användare
    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetWinteringsByUser(string userId)
    {
        var reports = await _context.Winterings
            .Where(w => w.UserId == userId)
            .ToListAsync();

        return Ok(reports);
    }

    //Uppdaterar en invintringsrapport
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateWintering(int id, WinteringModel updatedWintering)
    {
        var report = await _context.Winterings.FindAsync(id);
        if (report == null)
            return NotFound("Invintringsrapporten kunde inte hittas.");

        report.Season = updatedWintering.Season;
        report.HiveCount = updatedWintering.HiveCount;

        await _context.SaveChangesAsync();

        return Ok(report);
    }

    //Raderr en invintringsrapport
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteWintering(int id)
    {
        var report = await _context.Winterings.FindAsync(id);
        if (report == null)
            return NotFound("Invintringsrapporten kunde inte hittas.");

        _context.Winterings.Remove(report);
        await _context.SaveChangesAsync();

        return Ok("Invintringsrapporten har tagits bort.");
    }
}
