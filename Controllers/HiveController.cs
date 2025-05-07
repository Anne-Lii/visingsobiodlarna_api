using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.DTOs;
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
    public async Task<IActionResult> CreateHive(HiveDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var apiary = await _context.Apiaries
       .FirstOrDefaultAsync(a => a.Id == dto.ApiaryId && a.UserId == userId);

        if (apiary == null)
            return Forbid("Du får inte lägga till kupor i denna bigård.");

        var model = new HiveModel
        {
            Name = dto.Name!,
            Description = dto.Description,
            StartYear = dto.StartYear,
            ApiaryId = dto.ApiaryId
        };

        _context.Hives.Add(model);
        await _context.SaveChangesAsync();

        return Ok(new HiveDto
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            StartYear = model.StartYear,
            ApiaryId = model.ApiaryId
        });
    }

    //Hämtar alla kupor (GET /api/hive)
    [HttpGet]
    [Authorize(Roles = "admin")]//Bara admin kommer åt denna
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

        var dtoList = hives.Select(h => new HiveDto
        {
            Id = h.Id,
            Name = h.Name,
            Description = h.Description,
            ApiaryId = h.ApiaryId
        }).ToList();

        return Ok(dtoList);

    }

    //Hämtar alla kupor för en user (GET /api/hive/by-user/{userId})
    [HttpGet("by-user/{userId}")]
    public async Task<IActionResult> GetHivesByUser(string userId)
    {
        var hives = await _context.Hives
            .Include(h => h.Apiary)
            .Where(h => h.Apiary != null && h.Apiary.UserId == userId)
            .Select(h => new HiveDto
            {
                Id = h.Id,
                Name = h.Name,
                ApiaryId = h.ApiaryId
            })
            .ToListAsync();

        return Ok(hives);
    }

    // Hämta en specifik kupa
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHiveById(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var hive = await _context.Hives
            .Include(h => h.Apiary)
            .FirstOrDefaultAsync(h => h.Id == id && h.Apiary != null && h.Apiary.UserId == userId);

        if (hive == null)
            return NotFound("Kupan hittades inte eller tillhör inte användaren.");

        return Ok(new HiveDto
        {
            Id = hive.Id,
            Name = hive.Name,
            Description = hive.Description,
            StartYear = hive.StartYear,
            ApiaryId = hive.ApiaryId
        });
    }


    //Uppdatera en kupa (PUT /api/hive/{id})
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHive(int id, HiveDto dto)
    {

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var hive = await _context.Hives
        .Include(h => h.Apiary)
        .FirstOrDefaultAsync(h => h.Id == id && h.Apiary != null && h.Apiary.UserId == userId);

        if (hive == null)
            return NotFound("Kupan kunde inte hittas eller tillhör inte användaren.");

        hive.Name = dto.Name!;
        hive.Description = dto.Description;
        await _context.SaveChangesAsync();

        return Ok(new HiveDto
        {
            Id = hive.Id,
            Name = hive.Name,
            ApiaryId = hive.ApiaryId
        });
    }

    //Radera en kupa (DELETE /api/hive/{id})
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHive(int id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var hive = await _context.Hives
        .Include(h => h.Apiary)
        .FirstOrDefaultAsync(h => h.Id == id && h.Apiary != null && h.Apiary.UserId == userId);

        if (hive == null)
            return NotFound("Kupan kunde inte hittas eller tillhör inte användaren.");

        _context.Hives.Remove(hive);
        await _context.SaveChangesAsync();

        return Ok("Kupan har tagits bort.");
    }
}
