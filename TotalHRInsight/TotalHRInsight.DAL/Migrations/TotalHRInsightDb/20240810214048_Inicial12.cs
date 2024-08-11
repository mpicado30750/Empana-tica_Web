using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class Inicial12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PrecioUnitario",
                table: "Producto",
                type: "double",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "PrecioUnitario",
                table: "Producto",
                type: "double(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double");
        }
    }
}
