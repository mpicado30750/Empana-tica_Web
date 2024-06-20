using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class IncidenciaInicial1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_AspNetUsers_UsuarioAsignacionId",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "UsuarioAsignacionID",
                table: "Permisos");

            migrationBuilder.UpdateData(
                table: "Permisos",
                keyColumn: "UsuarioAsignacionId",
                keyValue: null,
                column: "UsuarioAsignacionId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioAsignacionId",
                table: "Permisos",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_AspNetUsers_UsuarioAsignacionId",
                table: "Permisos",
                column: "UsuarioAsignacionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Permisos_AspNetUsers_UsuarioAsignacionId",
                table: "Permisos");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioAsignacionId",
                table: "Permisos",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "UsuarioAsignacionID",
                table: "Permisos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Permisos_AspNetUsers_UsuarioAsignacionId",
                table: "Permisos",
                column: "UsuarioAsignacionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
