using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Kräver att användaren är inloggad
public class MitesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MitesController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Skapa en ny kvalsterrapport
    [HttpPost]
    public async Task<IActionResult> CreateMiteReport(MitesModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Mites.Add(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }

    //Hämta alla kvalsterrapporter för en kupa
    [HttpGet("by-hive/{hiveId}")]
    public async Task<IActionResult> GetReportsByHive(int hiveId)
    {
        var reports = await _context.Mites
            .Where(r => r.HiveId == hiveId)
            .ToListAsync();

        return Ok(reports);
    }

    //Hämta alla kvalsterrapporter för en bigård
    [HttpGet("by-apiary/{apiaryId}")]
    public async Task<IActionResult> GetReportsByApiary(int apiaryId)
    {
        var reports = await _context.Mites
            .Include(r => r.Hive)
            .Where(r => r.Hive != null && r.Hive.ApiaryId == apiaryId)
            .ToListAsync();

        return Ok(reports);
    }

    //Uppdatera en kvalsterrapport
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMiteReport(int id, MitesModel updatedReport)
    {
        var report = await _context.Mites.FindAsync(id);
        if (report == null)
            return NotFound("Rapporten kunde inte hittas.");

        report.Year = updatedReport.Year;
        report.Week = updatedReport.Week;
        report.MiteCount = updatedReport.MiteCount;

        await _context.SaveChangesAsync();

        return Ok(report);
    }

    //Radera en kvalsterrapport
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMiteReport(int id)
    {
        var report = await _context.Mites.FindAsync(id);
        if (report == null)
            return NotFound("Rapporten kunde inte hittas.");

        _context.Mites.Remove(report);
        await _context.SaveChangesAsync();

        return Ok("Rapporten har tagits bort.");
    }
}
