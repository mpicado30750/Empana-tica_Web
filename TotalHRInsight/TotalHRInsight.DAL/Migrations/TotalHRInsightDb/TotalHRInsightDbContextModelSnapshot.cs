﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TotalHRInsight.DAL;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    [DbContext(typeof(TotalHRInsightDbContext))]
    partial class TotalHRInsightDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
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

                    b.Property<double>("Salario")
                        .HasColumnType("double");

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

            modelBuilder.Entity("TotalHRInsight.DAL.CierreCaja", b =>
                {
                    b.Property<int>("IdCierraCaja")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdCierraCaja"));

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("MontoTotal")
                        .HasColumnType("double(18,2)");

                    b.Property<int>("SucursalId")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdCierraCaja");

                    b.HasIndex("SucursalId");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("CierreCaja");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Deduccion", b =>
                {
                    b.Property<int>("IdDeduccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdDeduccion"));

                    b.Property<DateTime>("FechaDeduccion")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("MontoDeduccion")
                        .HasColumnType("double(18,2)");

                    b.Property<string>("NombreDeduccion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("TipoDeduccionId")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioAsignacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdDeduccion");

                    b.HasIndex("TipoDeduccionId");

                    b.HasIndex("UsuarioAsignacionId");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Deduccion");
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

            modelBuilder.Entity("TotalHRInsight.DAL.Gasto", b =>
                {
                    b.Property<int>("IdGasto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdGasto"));

                    b.Property<int>("CierreId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("MontoGasto")
                        .HasColumnType("double(18,2)");

                    b.Property<int>("TipoGastoId")
                        .HasColumnType("int");

                    b.HasKey("IdGasto");

                    b.HasIndex("CierreId");

                    b.HasIndex("TipoGastoId");

                    b.ToTable("Gasto");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Ingreso", b =>
                {
                    b.Property<int>("IdIngreso")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdIngreso"));

                    b.Property<int>("CierreId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("MontoIngreso")
                        .HasColumnType("double(18,2)");

                    b.Property<int>("TipoIngresoId")
                        .HasColumnType("int");

                    b.HasKey("IdIngreso");

                    b.HasIndex("CierreId");

                    b.HasIndex("TipoIngresoId");

                    b.ToTable("Ingreso");
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

                    b.Property<double>("Cantidad")
                        .HasColumnType("double");

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

                    b.Property<string>("UsuarioAsignacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdPermisos");

                    b.HasIndex("IdEstado");

                    b.HasIndex("IdTipoPermiso");

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

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("MontoTotal")
                        .HasColumnType("double(18,2)");

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

                    b.Property<double>("PrecioUnitario")
                        .HasColumnType("double(18,2)");

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

            modelBuilder.Entity("TotalHRInsight.DAL.Salario", b =>
                {
                    b.Property<int>("IdSalario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdSalario"));

                    b.Property<double>("SalarioBruto")
                        .HasColumnType("double(18,2)");

                    b.Property<double>("SalarioExtra")
                        .HasColumnType("double(18,2)");

                    b.Property<double>("SalarioNeto")
                        .HasColumnType("double(18,2)");

                    b.Property<string>("UsuarioAsignacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UsuarioCreacionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdSalario");

                    b.HasIndex("UsuarioAsignacionId");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Salario");
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

            modelBuilder.Entity("TotalHRInsight.DAL.TipoDeduccion", b =>
                {
                    b.Property<int>("IdTipoDeduccion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdTipoDeduccion"));

                    b.Property<string>("NombreDeduccion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdTipoDeduccion");

                    b.ToTable("TipoDeduccion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.TipoGasto", b =>
                {
                    b.Property<int>("IdTipoGasto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdTipoGasto"));

                    b.Property<string>("NombreGasto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdTipoGasto");

                    b.ToTable("TipoGasto");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.TipoIngreso", b =>
                {
                    b.Property<int>("IdTipoIngreso")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdTipoIngreso"));

                    b.Property<string>("NombreIngreso")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdTipoIngreso");

                    b.ToTable("TipoIngreso");
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

            modelBuilder.Entity("TotalHRInsight.DAL.CierreCaja", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Sucursal", "Sucursal")
                        .WithMany()
                        .HasForeignKey("SucursalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sucursal");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Deduccion", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.TipoDeduccion", "TipoDeduccion")
                        .WithMany("Deduccion")
                        .HasForeignKey("TipoDeduccionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

                    b.Navigation("TipoDeduccion");

                    b.Navigation("UsuarioAsignacion");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Gasto", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.CierreCaja", "CierreCaja")
                        .WithMany("Gasto")
                        .HasForeignKey("CierreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.TipoGasto", "TipoGasto")
                        .WithMany("Gasto")
                        .HasForeignKey("TipoGastoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CierreCaja");

                    b.Navigation("TipoGasto");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Ingreso", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.CierreCaja", "CierreCaja")
                        .WithMany("Ingreso")
                        .HasForeignKey("CierreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.TipoIngreso", "TipoIngreso")
                        .WithMany("Ingreso")
                        .HasForeignKey("TipoIngresoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CierreCaja");

                    b.Navigation("TipoIngreso");
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
                        .HasForeignKey("IdTipoPermiso")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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

            modelBuilder.Entity("TotalHRInsight.DAL.Salario", b =>
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

            modelBuilder.Entity("TotalHRInsight.DAL.Categoria", b =>
                {
                    b.Navigation("Productos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.CierreCaja", b =>
                {
                    b.Navigation("Gasto");

                    b.Navigation("Ingreso");
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

            modelBuilder.Entity("TotalHRInsight.DAL.TipoDeduccion", b =>
                {
                    b.Navigation("Deduccion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.TipoGasto", b =>
                {
                    b.Navigation("Gasto");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.TipoIngreso", b =>
                {
                    b.Navigation("Ingreso");
                });
#pragma warning restore 612, 618
        }
    }
}
