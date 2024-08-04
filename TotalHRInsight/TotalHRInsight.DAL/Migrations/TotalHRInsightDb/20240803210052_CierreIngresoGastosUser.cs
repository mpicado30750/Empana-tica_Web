using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class CierreIngresoGastosUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioCreacionId",
                table: "CierreCaja",
                type: "varchar(255)",
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CierreCaja_UsuarioCreacionId",
                table: "CierreCaja",
                column: "UsuarioCreacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CierreCaja_AspNetUsers_UsuarioCreacionId",
                table: "CierreCaja",
                column: "UsuarioCreacionId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CierreCaja_AspNetUsers_UsuarioCreacionId",
                table: "CierreCaja");

            migrationBuilder.DropIndex(
                name: "IX_CierreCaja_UsuarioCreacionId",
                table: "CierreCaja");

            migrationBuilder.DropColumn(
                name: "UsuarioCreacionId",
                table: "CierreCaja");
        }
    }
}
