using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class asistenciaControl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Longitud",
                table: "Asistencia",
                type: "float",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Asistencia");
        }
    }
}
