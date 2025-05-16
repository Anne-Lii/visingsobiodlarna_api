
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.DTOs;
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
    public async Task<IActionResult> CreateHarvest(HoneyHarvestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Sätter UserId från inloggad användare
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var model = new HoneyHarvestModel
        {
            UserId = userId,
            Year = dto.Year,
            HarvestDate = dto.HarvestDate,
            AmountKg = dto.AmountKg,
            IsTotalForYear = dto.IsTotalForYear,
            BatchId = dto.BatchId
        };

        _context.HoneyHarvests.Add(model);
        await _context.SaveChangesAsync();

        //Returnera DTO med id från modellen
        dto.Id = model.Id;

        return Ok(dto);
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
             .Select(h => new HoneyHarvestDto
             {
                 Id = h.Id,
                 Year = h.Year,
                 HarvestDate = h.HarvestDate,
                 AmountKg = h.AmountKg,
                 IsTotalForYear = h.IsTotalForYear,
                 BatchId = null
             })
        .ToListAsync();

        return Ok(harvests);
    }

    //Hämta skörderapporter för en användare och ett specifikt år för att sätta batchnummer
    [HttpGet]
    public async Task<IActionResult> GetHarvestsByYear([FromQuery] int year)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var harvests = await _context.HoneyHarvests
            .Where(h => h.UserId == userId && h.Year == year)
            .Select(h => new HoneyHarvestDto
            {
                Id = h.Id,
                Year = h.Year,
                HarvestDate = h.HarvestDate,
                AmountKg = h.AmountKg,
                IsTotalForYear = h.IsTotalForYear,
                BatchId = h.BatchId
            })
            .ToListAsync();

        return Ok(harvests);
    }

    //Uppdatera en skörderapport
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHarvest(int id, HoneyHarvestDto dto)
    {
        var harvest = await _context.HoneyHarvests.FindAsync(id);
        if (harvest == null)
            return NotFound("Skörderapporten kunde inte hittas.");

        harvest.Year = dto.Year;
        harvest.HarvestDate = dto.HarvestDate;
        harvest.AmountKg = dto.AmountKg;
        harvest.IsTotalForYear = dto.IsTotalForYear;

        await _context.SaveChangesAsync();

        return Ok(dto);
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