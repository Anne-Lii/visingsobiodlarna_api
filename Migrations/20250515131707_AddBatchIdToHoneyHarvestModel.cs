using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace visingsobiodlarna_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchIdToHoneyHarvestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AmountKg",
                table: "HoneyHarvests",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BatchId",
                table: "HoneyHarvests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "HoneyHarvests");

            migrationBuilder.AlterColumn<int>(
                name: "AmountKg",
                table: "HoneyHarvests",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }
    }
}
