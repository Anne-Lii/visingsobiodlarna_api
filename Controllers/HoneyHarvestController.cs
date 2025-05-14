
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //användaren måste vara inloggad
public class HoneyHarvestController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HoneyHarvestController(ApplicationDbContext context)
    {
        _context = context;
    }


    //Skapa en ny skörderapport
    [HttpPost]
    public async Task<IActionResult> CreateHarvest(HoneyHarvestModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Sätter UserId från inloggad användare
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        model.UserId = userId;

        _context.HoneyHarvests.Add(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }

    //Hämtar alla skörderapporter för en användare
    [HttpGet("by-user")]
    public async Task<IActionResult> GetHarvestsByUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var harvests = await _context.HoneyHarvests
            .Where(h => h.UserId == userId)
            .OrderBy(h => h.HarvestDate)
            .ToListAsync();

        return Ok(harvests);
    }

    //Uppdatera en skörderapport
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHarvest(int id, HoneyHarvestModel updatedHarvest)
    {
        var harvest = await _context.HoneyHarvests.FindAsync(id);
        if (harvest == null)
            return NotFound("Skörderapporten kunde inte hittas.");

        harvest.Year = updatedHarvest.Year;
        harvest.HarvestDate = updatedHarvest.HarvestDate;
        harvest.AmountKg = updatedHarvest.AmountKg;
        harvest.IsTotalForYear = updatedHarvest.IsTotalForYear;

        await _context.SaveChangesAsync();

        return Ok(harvest);
    }

    //ta bort en skörderapport
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHarvest(int id)
    {
        var harvest = await _context.HoneyHarvests.FindAsync(id);
        if (harvest == null)
            return NotFound("Skörderapporten kunde inte hittas.");

        _context.HoneyHarvests.Remove(harvest);
        await _context.SaveChangesAsync();

        return Ok("Skörderapporten har tagits bort.");
    }
}