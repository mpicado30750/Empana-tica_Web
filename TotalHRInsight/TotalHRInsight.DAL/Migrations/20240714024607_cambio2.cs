using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TotalHRInsight.DAL.Migrations
{
    /// <inheritdoc />
    public partial class cambio2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proveedor_Producto_ProductosIdProducto",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Proveedor_ProductosIdProducto",
                table: "Proveedor");

            migrationBuilder.DropColumn(
                name: "ProductosIdProducto",
                table: "Proveedor");

            migrationBuilder.AddColumn<int>(
                name: "ProveedorId",
                table: "Producto",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Producto_ProveedorId",
                table: "Producto",
                column: "ProveedorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Proveedor_ProveedorId",
                table: "Producto",
                column: "ProveedorId",
                principalTable: "Proveedor",
                principalColumn: "IdProveedor",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Proveedor_ProveedorId",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_ProveedorId",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "ProveedorId",
                table: "Producto");

            migrationBuilder.AddColumn<int>(
                name: "ProductosIdProducto",
                table: "Proveedor",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_ProductosIdProducto",
                table: "Proveedor",
                column: "ProductosIdProducto");

            migrationBuilder.AddForeignKey(
                name: "FK_Proveedor_Producto_ProductosIdProducto",
                table: "Proveedor",
                column: "ProductosIdProducto",
                principalTable: "Producto",
                principalColumn: "IdProducto");
        }
    }
}
