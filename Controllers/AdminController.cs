using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Data;
using visingsobiodlarna_backend.DTOs;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    //konstruktor
    public AdminController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    //Hämta alla användare
    [HttpGet("users")]
    public IActionResult GetAllUsers()
    {
        var users = _userManager.Users.Select(u => new {
            u.Id,
            u.Email,
            u.FullName,
            u.IsApprovedByAdmin
        });

        return Ok(users);
    }

    //Hämta ej godkända användare
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingUsers()
{
    try
    {
        var users = await _userManager.Users
            .Where(u => !u.IsApprovedByAdmin)
            .Select(u => new PendingUserDto
            {
                Id = u.Id,
                FullName = $"{u.FirstName} {u.LastName}",
                Email = u.Email
            })
            .ToListAsync();
            
        return Ok(users);
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internt serverfel: {ex.Message}");
    }
}

    //Godkänn användare
    [HttpPut("approve/{userId}")]
    public async Task<IActionResult> ApproveUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Användare hittades inte.");

        if (user.IsApprovedByAdmin) return BadRequest("Användaren är redan godkänd.");

        user.IsApprovedByAdmin = true;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded ? Ok("Användaren har godkänts.") : BadRequest("Fel vid godkännande.");
    }

    //Tilldela admin-roll (valfri)
    [HttpPut("make-admin/{userId}")]
    public async Task<IActionResult> PromoteToAdmin(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Användare hittades inte.");

        var result = await _userManager.AddToRoleAsync(user, "admin");
        return result.Succeeded ? Ok("Användaren har uppgraderats till admin.") : BadRequest("Fel vid rolluppgradering.");
    }

    //Ta bort användare
    [HttpDelete("delete/{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Användare hittades inte.");

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded ? Ok("Användaren har tagits bort.") : BadRequest("Kunde inte ta bort användaren.");
    }

    [HttpGet("missing-wintering/{season}")]
    public async Task<IActionResult> GetUsersMissingWintering(string season)
    {
        //Hämtar alla godkända användare
        var allUsers = await _userManager.Users
            .Where(u => u.IsApprovedByAdmin)
            .ToListAsync();

        //Hämtar alla UserId som HAR rapporterat invintring för vald vinter
        var reportedUserIds = await _context.Winterings
            .Where(w => w.Season == season)
            .Select(w => w.UserId)
            .Distinct()
            .ToListAsync();

        //Filtrerar fram användare som saknar rapport med fullständigt namn och epostadress.
        var usersMissingReport = allUsers
            .Where(u => !reportedUserIds.Contains(u.Id))
            .Select(u => new {
                u.FullName,
                u.Email
            })
            .ToList();

        if (usersMissingReport.Count == 0)
        {
            return Ok("Alla användare har rapporterat invintrade kupor för " + season + ".");
        }

        return Ok(usersMissingReport);
    }
}