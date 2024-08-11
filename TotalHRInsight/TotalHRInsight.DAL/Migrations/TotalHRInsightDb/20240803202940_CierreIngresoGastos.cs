using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class CierreIngresoGastos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CierreCaja",
                columns: table => new
                {
                    IdCierraCaja = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SucursalId = table.Column<int>(type: "int", nullable: false),
                    MontoTotal = table.Column<double>(type: "double(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CierreCaja", x => x.IdCierraCaja);
                    table.ForeignKey(
                        name: "FK_CierreCaja_Sucursales_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Sucursales",
                        principalColumn: "IdSucursal",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoGasto",
                columns: table => new
                {
                    IdTipoGasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreGasto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoGasto", x => x.IdTipoGasto);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoIngreso",
                columns: table => new
                {
                    IdTipoIngreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreIngreso = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoIngreso", x => x.IdTipoIngreso);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Gasto",
                columns: table => new
                {
                    IdGasto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TipoGastoId = table.Column<int>(type: "int", nullable: false),
                    MontoGasto = table.Column<double>(type: "double(18,2)", nullable: false),
                    CierreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gasto", x => x.IdGasto);
                    table.ForeignKey(
                        name: "FK_Gasto_CierreCaja_CierreId",
                        column: x => x.CierreId,
                        principalTable: "CierreCaja",
                        principalColumn: "IdCierraCaja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gasto_TipoGasto_TipoGastoId",
                        column: x => x.TipoGastoId,
                        principalTable: "TipoGasto",
                        principalColumn: "IdTipoGasto",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ingreso",
                columns: table => new
                {
                    IdIngreso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fecha = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TipoIngresoId = table.Column<int>(type: "int", nullable: false),
                    MontoIngreso = table.Column<double>(type: "double(18,2)", nullable: false),
                    CierreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingreso", x => x.IdIngreso);
                    table.ForeignKey(
                        name: "FK_Ingreso_CierreCaja_CierreId",
                        column: x => x.CierreId,
                        principalTable: "CierreCaja",
                        principalColumn: "IdCierraCaja",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ingreso_TipoIngreso_TipoIngresoId",
                        column: x => x.TipoIngresoId,
                        principalTable: "TipoIngreso",
                        principalColumn: "IdTipoIngreso",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CierreCaja_SucursalId",
                table: "CierreCaja",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Gasto_CierreId",
                table: "Gasto",
                column: "CierreId");

            migrationBuilder.CreateIndex(
                name: "IX_Gasto_TipoGastoId",
                table: "Gasto",
                column: "TipoGastoId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingreso_CierreId",
                table: "Ingreso",
                column: "CierreId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingreso_TipoIngresoId",
                table: "Ingreso",
                column: "TipoIngresoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Gasto");

            migrationBuilder.DropTable(
                name: "Ingreso");

            migrationBuilder.DropTable(
                name: "TipoGasto");

            migrationBuilder.DropTable(
                name: "CierreCaja");

            migrationBuilder.DropTable(
                name: "TipoIngreso");
        }
    }
}
