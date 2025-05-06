using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.DTOs;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //Användaren måste vara inloggad för att använda denna controller
public class ApiaryController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ApiaryController(ApplicationDbContext context)
    {
        _context = context;
    }

    //Skapa en ny bigård (POST /api/apiary)
    [HttpPost]
    public async Task<IActionResult> CreateApiary(ApiaryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //Hämtar UserId från token och sätt den automatiskt
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null)
            return Unauthorized();

        var apiary = new ApiaryModel
        {
            Name = dto.Name,
            Location = dto.Location,
            UserId = userId
        };

        _context.Apiaries.Add(apiary);
        await _context.SaveChangesAsync();

        var result = new ApiaryDto
        {
            Id = apiary.Id,
            Name = apiary.Name!,
            Location = apiary.Location!,
            HiveCount = 0
        };

        return Ok(result);
    }

    //Hämtar alla bigårdar (GET /api/apiary)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiaries = await _context.Apiaries.Include(a => a.Hives).ToListAsync();
        return Ok(apiaries);
    }

    //hämtar inloggad användares alla bigårdar
    [HttpGet("my")]
    public async Task<IActionResult> GetMyApiaries()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var apiaries = await _context.Apiaries
            .Where(a => a.UserId == userId)
            .Include(a => a.Hives)
            .Select(a => new ApiaryDto
            {
                Id = a.Id,
                Name = a.Name!,
                Location = a.Location!,
                HiveCount = a.Hives.Count
            })
            .ToListAsync();

        return Ok(apiaries);
    }

    //Hämtar en specifik bigård med ID (GET /api/apiary/{id})
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        var apiary = await _context.Apiaries
            .Include(a => a.Hives)
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (apiary == null)
            return NotFound("Bigården kunde inte hittas.");

        var dto = new ApiaryDto
        {
            Id = apiary.Id,
            Name = apiary.Name!,
            Location = apiary.Location!,
            HiveCount = apiary.Hives.Count
        };

        return Ok(dto);
    }

    //Redigera bigård (PUT /api/apiary/{id})
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateApiary(int id, ApiaryModel updatedApiary)
    {
        var apiary = await _context.Apiaries.FindAsync(id);
        if (apiary == null)
            return NotFound("Bigården kunde inte hittas.");

        apiary.Name = updatedApiary.Name;
        apiary.Location = updatedApiary.Location;

        _context.Apiaries.Update(apiary);
        await _context.SaveChangesAsync();

        return Ok(apiary);
    }

    //Radera en bigård (DELETE /api/apiary/{id})
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApiary(int id)
    {
        var apiary = await _context.Apiaries.FindAsync(id);
        if (apiary == null)
            return NotFound("Bigården kunde inte hittas.");

        _context.Apiaries.Remove(apiary);
        await _context.SaveChangesAsync();

        return Ok("Bigården har tagits bort.");
    }


}