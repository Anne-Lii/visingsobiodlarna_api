using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
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
    public IActionResult GetPendingUsers()
    {
        var users = _userManager.Users
            .Where(u => !u.IsApprovedByAdmin)
            .Select(u => new {
                u.Id,
                u.Email,
                u.FullName
            });

        return Ok(users);
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
}