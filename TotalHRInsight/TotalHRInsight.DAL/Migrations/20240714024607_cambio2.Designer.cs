﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TotalHRInsight.DAL;

#nullable disable

namespace TotalHRInsight.DAL.Migrations
{
    [DbContext(typeof(TotalHRInsightDbContext))]
    [Migration("20240714024607_cambio2")]
    partial class cambio2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("TotalHRInsight.DAL.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Estado")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("FechaNacimiento")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaRegistro")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext");

                    b.Property<int>("NumeroTelefono")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PrimerApellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<float>("Salario")
                        .HasColumnType("float(18,4)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<string>("SegundoApellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext");

                    b.Property<int>("idSucursal")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("idSucursal");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Asistencia", b =>
                {
                    b.Property<int>("IdAsistencia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdAsistencia"));

                    b.Property<DateTime>("FechaEntrada")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaSalida")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UbicacionEntrada")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UbicacionSalida")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdAsistencia");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Asistencia");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Categoria", b =>
                {
                    b.Property<int>("IdCategoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdCategoria"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("NombreCategoria")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("IdCategoria");

                    b.ToTable("Categoria");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Estado", b =>
                {
                    b.Property<int>("IdEstado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdEstado"));

                    b.Property<string>("EstadoSolicitud")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdEstado");

                    b.ToTable("Estados");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Inventario", b =>
                {
                    b.Property<int>("IdInventario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdInventario"));

                    b.Property<int>("CantidadDisponible")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaModificacion")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("ProductoId")
                        .HasColumnType("int");

                    b.Property<int>("SucursalId")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioCreacionid")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UsuarioModificacionid")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdInventario");

                    b.HasIndex("ProductoId");

                    b.HasIndex("SucursalId");

                    b.HasIndex("UsuarioCreacionid");

                    b.HasIndex("UsuarioModificacionid");

                    b.ToTable("Inventario");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Medida", b =>
                {
                    b.Property<int>("IdMedida")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdMedida"));

                    b.Property<string>("NombreMedida")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdMedida");

                    b.ToTable("Medidas");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdPedido"));

                    b.Property<DateTime>("FechaEntrega")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaPedido")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int>("IdSucursal")
                        .HasColumnType("int");

                    b.Property<double>("MontoTotal")
                        .HasColumnType("double(18,2)");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdPedido");

                    b.HasIndex("IdEstado");

                    b.HasIndex("IdSucursal");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Pedidos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.PedidosProductos", b =>
                {
                    b.Property<int>("PedidosProductosID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("PedidosProductosID"));

                    b.Property<float>("Cantidad")
                        .HasColumnType("float");

                    b.Property<int>("PedidoID")
                        .HasColumnType("int");

                    b.Property<int>("ProductosID")
                        .HasColumnType("int");

                    b.HasKey("PedidosProductosID");

                    b.HasIndex("PedidoID");

                    b.HasIndex("ProductosID");

                    b.ToTable("PedidoProductos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Permiso", b =>
                {
                    b.Property<int>("IdPermisos")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdPermisos"));

                    b.Property<int>("CantidadDias")
                        .HasColumnType("int");

                    b.Property<string>("Comentario")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("IdEstado")
                        .HasColumnType("int");

                    b.Property<int>("IdTipoPermiso")
                        .HasColumnType("int");

                    b.Property<string>("MotivoAdmin")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("TipoPermisosIdTipoPermiso")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioAsignacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdPermisos");

                    b.HasIndex("IdEstado");

                    b.HasIndex("TipoPermisosIdTipoPermiso");

                    b.HasIndex("UsuarioAsignacionId");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Permisos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Planilla", b =>
                {
                    b.Property<int>("IdPlanilla")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdPlanilla"));

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("MontoTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UsuarioAsignacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdPlanilla");

                    b.HasIndex("UsuarioAsignacionId");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Planillas");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdProducto"));

                    b.Property<int>("CategoriaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaVencimiento")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MedidasId")
                        .HasColumnType("int");

                    b.Property<string>("NombreProducto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("PrecioUnitario")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ProveedorId")
                        .HasColumnType("int");

                    b.HasKey("IdProducto");

                    b.HasIndex("CategoriaId");

                    b.HasIndex("MedidasId");

                    b.HasIndex("ProveedorId");

                    b.ToTable("Producto");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Proveedor", b =>
                {
                    b.Property<int>("IdProveedor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdProveedor"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("MetodoPago")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("NombreProveedor")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdProveedor");

                    b.ToTable("Proveedor");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Sucursal", b =>
                {
                    b.Property<int>("IdSucursal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdSucursal"));

                    b.Property<double>("Latitud")
                        .HasColumnType("double");

                    b.Property<double>("Longitud")
                        .HasColumnType("double");

                    b.Property<string>("NombreSucursal")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UbicacionSucursal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdSucursal");

                    b.ToTable("Sucursales");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.TipoPermiso", b =>
                {
                    b.Property<int>("IdTipoPermiso")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdTipoPermiso"));

                    b.Property<string>("NombrePermiso")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdTipoPermiso");

                    b.ToTable("TiposPermisos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.ApplicationUser", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Sucursal", "Sucursal")
                        .WithMany()
                        .HasForeignKey("idSucursal")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sucursal");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Asistencia", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Inventario", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Producto", "Producto")
                        .WithMany("Inventario")
                        .HasForeignKey("ProductoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.Sucursal", "Sucursal")
                        .WithMany()
                        .HasForeignKey("SucursalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioModificacion")
                        .WithMany()
                        .HasForeignKey("UsuarioModificacionid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Producto");

                    b.Navigation("Sucursal");

                    b.Navigation("UsuarioCreacion");

                    b.Navigation("UsuarioModificacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Pedido", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.Sucursal", "Sucursal")
                        .WithMany()
                        .HasForeignKey("IdSucursal")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");

                    b.Navigation("Sucursal");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.PedidosProductos", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Pedido", "Pedido")
                        .WithMany("PedidosProductos")
                        .HasForeignKey("PedidoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.Producto", "Producto")
                        .WithMany("PedidosProductos")
                        .HasForeignKey("ProductosID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pedido");

                    b.Navigation("Producto");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Permiso", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Estado", "Estado")
                        .WithMany()
                        .HasForeignKey("IdEstado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.TipoPermiso", "TipoPermisos")
                        .WithMany()
                        .HasForeignKey("TipoPermisosIdTipoPermiso");

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioAsignacion")
                        .WithMany()
                        .HasForeignKey("UsuarioAsignacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Estado");

                    b.Navigation("TipoPermisos");

                    b.Navigation("UsuarioAsignacion");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Planilla", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioAsignacion")
                        .WithMany()
                        .HasForeignKey("UsuarioAsignacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UsuarioAsignacion");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Producto", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Categoria", "Categorias")
                        .WithMany("Productos")
                        .HasForeignKey("CategoriaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.Medida", "Medidas")
                        .WithMany("Productos")
                        .HasForeignKey("MedidasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.Proveedor", "Proveedor")
                        .WithMany("Producto")
                        .HasForeignKey("ProveedorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorias");

                    b.Navigation("Medidas");

                    b.Navigation("Proveedor");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Categoria", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Medida", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Pedido", b =>
                {
                    b.Navigation("PedidosProductos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Producto", b =>
                {
                    b.Navigation("Inventario");

                    b.Navigation("PedidosProductos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Proveedor", b =>
                {
                    b.Navigation("Producto");
                });
#pragma warning restore 612, 618
        }
    }
}