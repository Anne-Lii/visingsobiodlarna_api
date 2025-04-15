using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Models;
using visingsobiodlarna_backend.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using visingsobiodlarna_backend.Services;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


//Registrerar autentisering och anger JWT som standard
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

//Lägger till stöd för JWT-baserad autentisering
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,//Kontroll att token är utfärdad av rätt källa
        ValidateAudience = true,//Kontroll att token är avsedd för rätt mottagare
        ValidateLifetime = true, //Kontroll att token inte har gått ut
        ValidateIssuerSigningKey = true,//Kontroll att token är korrekt validerad

        //hämtar inställningar från appsettings.json
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),//Hemlig nyckel som används för att validera token
        RoleClaimType = ClaimTypes.Role
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();////kollar om användaren är inloggad och har JWT
app.UseAuthorization();//kollar om användaren har rätt roll/rättigheter
app.MapControllers();

async Task SeedRolesAsync()
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    await RoleInitializer.SeedRolesAsync(services);
}

app.MapGet("/api/test", () => "API fungerar!");

// Anropa metoden innan app.Run()
await SeedRolesAsync();

app.Run();
