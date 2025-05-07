using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.DTOs;
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
    public async Task<IActionResult> CreateMiteReport([FromBody] MiteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var model = new MitesModel
        {
            HiveId = dto.HiveId,
            Year = dto.Year,
            Week = dto.Week,
            MiteCount = dto.MiteCount
        };

        _context.Mites.Add(model);
        await _context.SaveChangesAsync();

        return Ok(new MiteDto
        {
            HiveId = model.HiveId,
            Year = model.Year,
            Week = model.Week,
            MiteCount = model.MiteCount
        });
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
    public async Task<IActionResult> UpdateMiteReport(int id, [FromBody] MiteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var report = await _context.Mites.FindAsync(id);
        if (report == null)
            return NotFound("Rapporten kunde inte hittas.");

        report.Year = dto.Year;
        report.Week = dto.Week;
        report.MiteCount = dto.MiteCount;

        await _context.SaveChangesAsync();

        return Ok(new MiteDto
        {
            HiveId = report.HiveId,
            Year = report.Year,
            Week = report.Week,
            MiteCount = report.MiteCount
        });
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
