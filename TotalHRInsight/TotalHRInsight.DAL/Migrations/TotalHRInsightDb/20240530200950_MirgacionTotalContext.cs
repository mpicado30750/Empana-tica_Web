using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class MirgacionTotalContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PrimwerApellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SegundoApellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaNacimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    NumeroTelefono = table.Column<float>(type: "float", nullable: false),
                    Salario = table.Column<float>(type: "float", nullable: false),
                    Estado = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Sucursales",
                columns: table => new
                {
                    IdSucursal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreSucursal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UbicacionSucursal = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sucursales", x => x.IdSucursal);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Planillas",
                columns: table => new
                {
                    IdPlanilla = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    MontoTotal = table.Column<float>(type: "float", nullable: false),
                    UsuarioCrecionId = table.Column<int>(type: "int", nullable: false),
                    IdAsistencia = table.Column<int>(type: "int", nullable: false),
                    IdPermiso = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planillas", x => x.IdPlanilla);
                    table.ForeignKey(
                        name: "FK_Planillas_ApplicationUser_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaPedido = table.Column<DateOnly>(type: "date", nullable: false),
                    CantidadPedido = table.Column<int>(type: "int", nullable: false),
                    MontoTotal = table.Column<float>(type: "float", nullable: false),
                    UsuarioCrecionId = table.Column<int>(type: "int", nullable: false),
                    IdSucursal = table.Column<int>(type: "int", nullable: false),
                    SucursalIdSucursal = table.Column<int>(type: "int", nullable: true),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_ApplicationUser_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pedidos_Sucursales_SucursalIdSucursal",
                        column: x => x.SucursalIdSucursal,
                        principalTable: "Sucursales",
                        principalColumn: "IdSucursal");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asistencia",
                columns: table => new
                {
                    idAsistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaEntrada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioCrecionId = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlanillaIdPlanilla = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencia", x => x.idAsistencia);
                    table.ForeignKey(
                        name: "FK_Asistencia_ApplicationUser_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Asistencia_Planillas_PlanillaIdPlanilla",
                        column: x => x.PlanillaIdPlanilla,
                        principalTable: "Planillas",
                        principalColumn: "IdPlanilla");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    idPermisos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    TipoPermiso = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CantidadDias = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioCrecionId = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PlanillaIdPlanilla = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.idPermisos);
                    table.ForeignKey(
                        name: "FK_Permisos_ApplicationUser_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Permisos_Planillas_PlanillaIdPlanilla",
                        column: x => x.PlanillaIdPlanilla,
                        principalTable: "Planillas",
                        principalColumn: "IdPlanilla");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreProducto = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    FechaVencimiento = table.Column<DateOnly>(type: "date", nullable: false),
                    PrecioUnitario = table.Column<float>(type: "float", nullable: false),
                    PedidoIdPedido = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Productos_Pedidos_PedidoIdPedido",
                        column: x => x.PedidoIdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_PlanillaIdPlanilla",
                table: "Asistencia",
                column: "PlanillaIdPlanilla");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_UsuarioCreacionId",
                table: "Asistencia",
                column: "UsuarioCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_SucursalIdSucursal",
                table: "Pedidos",
                column: "SucursalIdSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioCreacionId",
                table: "Pedidos",
                column: "UsuarioCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_PlanillaIdPlanilla",
                table: "Permisos",
                column: "PlanillaIdPlanilla");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_UsuarioCreacionId",
                table: "Permisos",
                column: "UsuarioCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Planillas_UsuarioCreacionId",
                table: "Planillas",
                column: "UsuarioCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_PedidoIdPedido",
                table: "Productos",
                column: "PedidoIdPedido");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencia");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Planillas");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Sucursales");
        }
    }
}
