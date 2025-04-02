using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
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
    public async Task<IActionResult> CreateApiary(ApiaryModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _context.Apiaries.Add(model);
        await _context.SaveChangesAsync();

        return Ok(model);
    }

    //Hämtar alla bigårdar (GET /api/apiary)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var apiaries = await _context.Apiaries.Include(a => a.Hives).ToListAsync();
        return Ok(apiaries);
    }

    //Hämtar en specifik bigård med ID (GET /api/apiary/{id})
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var apiary = await _context.Apiaries
            .Include(a => a.Hives)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (apiary == null)
            return NotFound("Bigården kunde inte hittas.");

        return Ok(apiary);
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

    //Radera en bigård DELETE (DELETE /api/apiary/{id})
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