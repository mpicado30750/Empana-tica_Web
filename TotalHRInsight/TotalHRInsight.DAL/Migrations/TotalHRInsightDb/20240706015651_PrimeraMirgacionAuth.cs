using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    /// <inheritdoc />
    public partial class PrimeraMirgacionAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Estados",
                columns: table => new
                {
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EstadoSolicitud = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados", x => x.IdEstado);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Medidas",
                columns: table => new
                {
                    IdMedida = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreMedida = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medidas", x => x.IdMedida);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "Sucursales",
            //    columns: table => new
            //    {
            //        IdSucursal = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //        NombreSucursal = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        UbicacionSucursal = table.Column<string>(type: "longtext", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Longitud = table.Column<double>(type: "double", nullable: false),
            //        Latitud = table.Column<double>(type: "double", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Sucursales", x => x.IdSucursal);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TiposPermisos",
                columns: table => new
                {
                    IdTipoPermiso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombrePermiso = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposPermisos", x => x.IdTipoPermiso);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Producto",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NombreProducto = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descripcion = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaVencimiento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MedidasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producto", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK_Producto_Medidas_MedidasId",
                        column: x => x.MedidasId,
                        principalTable: "Medidas",
                        principalColumn: "IdMedida",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "varchar(255)", nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Nombre = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        PrimerApellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        SegundoApellido = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        FechaNacimiento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        FechaRegistro = table.Column<DateTime>(type: "datetime(6)", nullable: false),
            //        NumeroTelefono = table.Column<int>(type: "int", nullable: false),
            //        Salario = table.Column<float>(type: "float(18,4)", nullable: false),
            //        Estado = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //        idSucursal = table.Column<int>(type: "int", nullable: false),
            //        UserName = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        NormalizedUserName = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        Email = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        NormalizedEmail = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //        PasswordHash = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
            //            .Annotation("MySql:CharSet", "utf8mb4"),
            //        PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
            //        LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
            //        AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUsers_Sucursales_idSucursal",
            //            column: x => x.idSucursal,
            //            principalTable: "Sucursales",
            //            principalColumn: "IdSucursal",
            //            onDelete: ReferentialAction.Restrict);
            //    })
            //    .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asistencia",
                columns: table => new
                {
                    IdAsistencia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaEntrada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaSalida = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UbicacionEntrada = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UbicacionSalida = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asistencia", x => x.IdAsistencia);
                    table.ForeignKey(
                        name: "FK_Asistencia_AspNetUsers_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Inventario",
                columns: table => new
                {
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioCreacionid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioModificacionid = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FechaCreacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaModificacion = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CantidadDisponible = table.Column<int>(type: "int", nullable: false),
                    SucursalId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventario", x => x.IdInventario);
                    table.ForeignKey(
                        name: "FK_Inventario_AspNetUsers_UsuarioCreacionid",
                        column: x => x.UsuarioCreacionid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventario_AspNetUsers_UsuarioModificacionid",
                        column: x => x.UsuarioModificacionid,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventario_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventario_Sucursales_SucursalId",
                        column: x => x.SucursalId,
                        principalTable: "Sucursales",
                        principalColumn: "IdSucursal",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaPedido = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaEntrega = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdSucursal = table.Column<int>(type: "int", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false),
                    MontoTotal = table.Column<double>(type: "double(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK_Pedidos_AspNetUsers_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Pedidos_Sucursales_IdSucursal",
                        column: x => x.IdSucursal,
                        principalTable: "Sucursales",
                        principalColumn: "IdSucursal",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    IdPermisos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CantidadDias = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MotivoAdmin = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdTipoPermiso = table.Column<int>(type: "int", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioAsignacionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdEstado = table.Column<int>(type: "int", nullable: false),
                    TipoPermisosIdTipoPermiso = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.IdPermisos);
                    table.ForeignKey(
                        name: "FK_Permisos_AspNetUsers_UsuarioAsignacionId",
                        column: x => x.UsuarioAsignacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permisos_AspNetUsers_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permisos_Estados_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "Estados",
                        principalColumn: "IdEstado",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permisos_TiposPermisos_TipoPermisosIdTipoPermiso",
                        column: x => x.TipoPermisosIdTipoPermiso,
                        principalTable: "TiposPermisos",
                        principalColumn: "IdTipoPermiso");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Planillas",
                columns: table => new
                {
                    IdPlanilla = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FechaInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MontoTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsuarioCreacionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UsuarioAsignacionId = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planillas", x => x.IdPlanilla);
                    table.ForeignKey(
                        name: "FK_Planillas_AspNetUsers_UsuarioAsignacionId",
                        column: x => x.UsuarioAsignacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Planillas_AspNetUsers_UsuarioCreacionId",
                        column: x => x.UsuarioCreacionId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PedidoProductos",
                columns: table => new
                {
                    PedidosProductosID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductosID = table.Column<int>(type: "int", nullable: false),
                    PedidoID = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<float>(type: "float", nullable: false),
                    Medida = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoProductos", x => x.PedidosProductosID);
                    table.ForeignKey(
                        name: "FK_PedidoProductos_Pedidos_PedidoID",
                        column: x => x.PedidoID,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidoProductos_Producto_ProductosID",
                        column: x => x.ProductosID,
                        principalTable: "Producto",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Asistencia_UsuarioCreacionId",
                table: "Asistencia",
                column: "UsuarioCreacionId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUsers_idSucursal",
            //    table: "AspNetUsers",
            //    column: "idSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_ProductoId",
                table: "Inventario",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_SucursalId",
                table: "Inventario",
                column: "SucursalId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_UsuarioCreacionid",
                table: "Inventario",
                column: "UsuarioCreacionid");

            migrationBuilder.CreateIndex(
                name: "IX_Inventario_UsuarioModificacionid",
                table: "Inventario",
                column: "UsuarioModificacionid");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProductos_PedidoID",
                table: "PedidoProductos",
                column: "PedidoID");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoProductos_ProductosID",
                table: "PedidoProductos",
                column: "ProductosID");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdEstado",
                table: "Pedidos",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdSucursal",
                table: "Pedidos",
                column: "IdSucursal");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_UsuarioCreacionId",
                table: "Pedidos",
                column: "UsuarioCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_IdEstado",
                table: "Permisos",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_TipoPermisosIdTipoPermiso",
                table: "Permisos",
                column: "TipoPermisosIdTipoPermiso");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_UsuarioAsignacionId",
                table: "Permisos",
                column: "UsuarioAsignacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Permisos_UsuarioCreacionId",
                table: "Permisos",
                column: "UsuarioCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Planillas_UsuarioAsignacionId",
                table: "Planillas",
                column: "UsuarioAsignacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Planillas_UsuarioCreacionId",
                table: "Planillas",
                column: "UsuarioCreacionId");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_MedidasId",
                table: "Producto",
                column: "MedidasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asistencia");

            migrationBuilder.DropTable(
                name: "Inventario");

            migrationBuilder.DropTable(
                name: "PedidoProductos");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Planillas");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Producto");

            migrationBuilder.DropTable(
                name: "TiposPermisos");

            //migrationBuilder.DropTable(
            //    name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Estados");

            migrationBuilder.DropTable(
                name: "Medidas");

            //migrationBuilder.DropTable(
            //    name: "Sucursales");
        }
    }
}
