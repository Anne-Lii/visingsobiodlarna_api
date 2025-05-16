using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using visingsobiodlarna_backend.Models;

namespace visingsobiodlarna_backend.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    //Db set
    public DbSet<ApiaryModel> Apiaries { get; set; } //Bigårdar
    public DbSet<HiveModel> Hives { get; set; }//Kupor
    public DbSet<MitesModel> Mites { get; set; }//Varrova kvalster
    public DbSet<WinteringModel> Winterings { get; set; }//invintrade kupor
    public DbSet<HoneyHarvestModel> HoneyHarvests { get; set; }//Honungsskörd
    public DbSet<CalenderModel> CalenderModels { get; set; }//Kalenderhändelser
    public DbSet<NewsModel> NewsModels { get; set; }//Nyheter
    public DbSet<DocumentModel> Documents { get; set; }//Dokument



    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Specificerar decimal-precision för AmountKg i HoneyHarvestModel
        builder.Entity<HoneyHarvestModel>()
        .Property(h => h.AmountKg)
        .HasColumnType("decimal(8,2)");
    }
}