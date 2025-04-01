using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using visingsobiodlarna_backend.Models;
using visingsobiodlarna_backend.DTOs;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsApprovedByAdmin = false// Alla nya användarkonton måste godkännas av admin
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            return Ok("Användaren registrerad. Väntar på godkännande. (kan ta upp till 24 timmar)");
        }

        return BadRequest(result.Errors);
    }

}