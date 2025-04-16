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

//CORS-tjänst
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000") //Tillåt begärningar från localhost:3000
              .AllowAnyHeader() //Tillåt alla headers
              .AllowAnyMethod(); //Tillåt alla HTTP-metoder (GET, POST, PUT, DELETE, etc.)
    });
});

//Konfiguration för hur JSON ska hanteras i APIet
builder.Services.AddControllers().AddJsonOptions(options => 
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;//förhindrar oändlig cirkulation vid serialisering
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;//matchar inkommande JSON-fält camelCase-Pascal
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

//Registrerar autentisering och ange JWT som standard
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        RoleClaimType = ClaimTypes.Role
    };
});

var app = builder.Build();



//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowLocalhost");//Aktiverar CORS för localhost
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


app.Use(async (context, next) =>
{
    Console.WriteLine($"Request: {context.Request.Method} {context.Request.Path}");
    await next();
});

app.Run();
