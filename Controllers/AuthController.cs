using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using visingsobiodlarna_backend.Models;
using visingsobiodlarna_backend.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace visingsobiodlarna_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _env;

    public AuthController(UserManager<ApplicationUser> userManager, IConfiguration config, IWebHostEnvironment env)
    {
        _userManager = userManager;
        _config = config;
        _env = env;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {

        Console.WriteLine("Incoming register model:");
        Console.WriteLine($"Email: {model?.Email}");
        Console.WriteLine($"FirstName: {model?.FirstName}");
        Console.WriteLine($"LastName: {model?.LastName}");
        Console.WriteLine($"Password: {model?.Password}");

        //validering 
        if (string.IsNullOrWhiteSpace(model.Email))
        {
            return BadRequest("E-postadress är obligatorisk.");
        }

        if (!new EmailAddressAttribute().IsValid(model.Email))
        {
            return BadRequest("Ogiltig e-postadress.");
        }

        if (string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("Lösenord är obligatoriskt.");
        }

        if (string.IsNullOrWhiteSpace(model.FirstName) || string.IsNullOrWhiteSpace(model.LastName))
        {
            return BadRequest("Förnamn och efternamn är obligatoriska.");
        }

        //skapar användaren
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
            await _userManager.AddToRoleAsync(user, "member");
            return Ok("Användaren registrerad. Väntar på godkännande. (kan ta upp till 24 timmar)");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return Unauthorized("Fel e-postadress eller lösenord.");
        }

        if (!user.IsApprovedByAdmin)
        {
            return Unauthorized("Kontot är ännu inte godkänt av administratör.");
        }

        var userRoles = await _userManager.GetRolesAsync(user);//hämtar användarens roll från databasen

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.FullName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        // Lägger till varje roll som en egen claim i token
        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //skickar JWT som en HttpOnly-cookie
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,//false i utveckling och true om INTE i utvecklingsläge
            SameSite = SameSiteMode.None, //tillåter backend och frontend på olika domäner
            Expires = DateTime.UtcNow.AddHours(1)
        };

        Response.Cookies.Append("jwt", tokenString, cookieOptions);

        return Ok(new { message = "Inloggning lyckades" });
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwt");
        return Ok(new { message = "Utloggad" });
    }

    [HttpGet("validate")]

    public IActionResult Validate()
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            return Ok(new { message = "Inloggad" });
        }

        return Unauthorized();
    }

}