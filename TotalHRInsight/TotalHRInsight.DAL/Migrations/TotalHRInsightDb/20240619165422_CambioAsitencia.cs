using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class CambioAsitencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Asistencia");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Asistencia");

            migrationBuilder.RenameColumn(
                name: "idAsistencia",
                table: "Asistencia",
                newName: "IdAsistencia");

            migrationBuilder.RenameColumn(
                name: "Ubicacion",
                table: "Asistencia",
                newName: "UbicacionSalida");

            migrationBuilder.AddColumn<string>(
                name: "UbicacionEntrada",
                table: "Asistencia",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UbicacionEntrada",
                table: "Asistencia");

            migrationBuilder.RenameColumn(
                name: "IdAsistencia",
                table: "Asistencia",
                newName: "idAsistencia");

            migrationBuilder.RenameColumn(
                name: "UbicacionSalida",
                table: "Asistencia",
                newName: "Ubicacion");

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                table: "Asistencia",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                table: "Asistencia",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
