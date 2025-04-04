using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace visingsobiodlarna_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddHoneyHarvestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HoneyHarvests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    HarvestDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AmountKg = table.Column<int>(type: "int", nullable: false),
                    IsTotalForYear = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoneyHarvests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoneyHarvests");
        }
    }
}
