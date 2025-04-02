using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
    {

    }

    //Db set
    public DbSet<Apiary> Apiaries { get; set; } //Bigårdar
}