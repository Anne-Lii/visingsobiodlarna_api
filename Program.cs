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
              .AllowAnyMethod()//Tillåt alla HTTP-metoder (GET, POST, PUT, DELETE, etc.)
              .AllowCredentials();
    });
});

//Krav på lösenord vid registrering
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;//krav på minst en siffra
    options.Password.RequireLowercase = false;//inget krav på små bokstäver
    options.Password.RequireUppercase = false;//inget krav på stora bokstäver
    options.Password.RequireNonAlphanumeric = false;//inget krav på specialtecken
    options.Password.RequiredLength = 6;//krav på minst 6 tecken
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //undviker serialiseringsfel vid objekt som referar till varandra hive - apiary - hive - 
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        
        //Automatisk konvertering till camelCase
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        
        //JSON hanterar stora/små bokstäver i property-namn
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
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

    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        );
    });
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
    
//kontroll av JWTkey
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT Key not found in configuration.");
}

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

    //Ta token från cookies om den inte finns i Authorization-headern
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt"))
            {
                context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
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
