using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //Kräver att användaren är inloggad
public class HiveController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HiveController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Skapa en kupa (POST /api/hive)
    [HttpPost]
    public async Task<IActionResult> CreateHive(HiveModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Hives.Add(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }

    //Hämtar alla kupor (GET /api/hive)
    [HttpGet]
    public async Task<IActionResult> GetAllHives()
    {
        var hives = await _context.Hives
            .Include(h => h.Apiary)
            .ToListAsync();

        return Ok(hives);
    }

    //Hämtar alla kupor för en viss bigård (GET /api/hive/by-apiary/1)
    [HttpGet("by-apiary/{apiaryId}")]
    public async Task<IActionResult> GetHivesByApiary(int apiaryId)
    {
        var hives = await _context.Hives
            .Where(h => h.ApiaryId == apiaryId)
            .ToListAsync();

        return Ok(hives);
    }

    //Hämtar alla kupor för en user (GET /api/hive/by-user/{userId})
    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetHivesByUser(string userId)
    {
        var hives = await _context.Hives
            .Include(h => h.Apiary)
            .Where(h => h.Apiary != null && h.Apiary.UserId == userId)
            .ToListAsync();

        return Ok(hives);
    }

    //Uppdatera en kupa (PUT /api/hive/{id})
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHive(int id, HiveModel updatedHive)
    {
        var hive = await _context.Hives.FindAsync(id);
        if (hive == null)
            return NotFound("Kupan kunde inte hittas.");

        hive.Name = updatedHive.Name;
        hive.ApiaryId = updatedHive.ApiaryId;

        _context.Hives.Update(hive);
        await _context.SaveChangesAsync();

        return Ok(hive);
    }

    //Radera en kupa (DELETE /api/hive/{id})
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHive(int id)
    {
        var hive = await _context.Hives.FindAsync(id);
        if (hive == null)
            return NotFound("Kupan kunde inte hittas.");

        _context.Hives.Remove(hive);
        await _context.SaveChangesAsync();

        return Ok("Kupan har tagits bort.");
    }
}
