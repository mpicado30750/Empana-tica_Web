﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TotalHRInsight.DAL;

#nullable disable

namespace TotalHRInsight.DAL.Migrations.TotalHRInsightDb
{
    [DbContext(typeof(TotalHRInsightDbContext))]
    [Migration("20240616223950_asistenciaControl")]
    partial class asistenciaControl
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

                    b.Property<DateOnly>("FechaNacimiento")
                        .HasColumnType("date");

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

                    b.Property<float>("NumeroTelefono")
                        .HasColumnType("float");

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

                    b.Property<decimal>("Salario")
                        .HasColumnType("decimal(18,4)");

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

                    b.HasKey("Id");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Asistencia", b =>
                {
                    b.Property<int>("idAsistencia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("idAsistencia"));

                    b.Property<DateTime>("FechaEntrada")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FechaSalida")
                        .HasColumnType("datetime(6)");

                    b.Property<float>("Longitud")
                        .HasColumnType("float");

                    b.Property<int?>("PlanillaIdPlanilla")
                        .HasColumnType("int");

                    b.Property<string>("UsuarioCreacionId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UsuarioCrecionId")
                        .HasColumnType("int");

                    b.HasKey("idAsistencia");

                    b.HasIndex("PlanillaIdPlanilla");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Asistencia");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Pedido", b =>
                {
                    b.Property<int>("IdPedido")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdPedido"));

                    b.Property<string>("EstadoPedido")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateOnly>("FechaPedido")
                        .HasColumnType("date");

                    b.Property<int>("IdSucursal")
                        .HasColumnType("int");

                    b.Property<float>("MontoTotal")
                        .HasColumnType("float");

                    b.Property<string>("UsuarioCrecionId")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdPedido");

                    b.HasIndex("IdSucursal");

                    b.HasIndex("UsuarioCrecionId");

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

                    b.Property<string>("Medida")
                        .IsRequired()
                        .HasColumnType("longtext");

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
                    b.Property<int>("idPermisos")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("idPermisos"));

                    b.Property<int>("CantidadDias")
                        .HasColumnType("int");

                    b.Property<bool>("Estado")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateOnly>("FechaFin")
                        .HasColumnType("date");

                    b.Property<DateOnly>("FechaInicio")
                        .HasColumnType("date");

                    b.Property<int?>("PlanillaIdPlanilla")
                        .HasColumnType("int");

                    b.Property<string>("TipoPermiso")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UsuarioCreacionId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UsuarioCrecionId")
                        .HasColumnType("int");

                    b.HasKey("idPermisos");

                    b.HasIndex("PlanillaIdPlanilla");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Permisos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Planilla", b =>
                {
                    b.Property<int>("IdPlanilla")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdPlanilla"));

                    b.Property<DateOnly>("FechaFin")
                        .HasColumnType("date");

                    b.Property<DateOnly>("FechaInicio")
                        .HasColumnType("date");

                    b.Property<int>("IdAsistencia")
                        .HasColumnType("int");

                    b.Property<int>("IdPermiso")
                        .HasColumnType("int");

                    b.Property<float>("MontoTotal")
                        .HasColumnType("float");

                    b.Property<string>("UsuarioCreacionId")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UsuarioCrecionId")
                        .HasColumnType("int");

                    b.HasKey("IdPlanilla");

                    b.HasIndex("UsuarioCreacionId");

                    b.ToTable("Planillas");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Producto", b =>
                {
                    b.Property<int>("IdProducto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdProducto"));

                    b.Property<int>("CantidadDisponible")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateOnly>("FechaVencimiento")
                        .HasColumnType("date");

                    b.Property<string>("NombreProducto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("PrecioUnitario")
                        .HasColumnType("float");

                    b.Property<float>("Unidad")
                        .HasColumnType("float");

                    b.HasKey("IdProducto");

                    b.ToTable("Productos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Sucursal", b =>
                {
                    b.Property<int>("IdSucursal")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("IdSucursal"));

                    b.Property<string>("NombreSucursal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("UbicacionSucursal")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("IdSucursal");

                    b.ToTable("Sucursales");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Asistencia", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Planilla", null)
                        .WithMany("Asistencias")
                        .HasForeignKey("PlanillaIdPlanilla");

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Pedido", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.Sucursal", "Sucursal")
                        .WithMany()
                        .HasForeignKey("IdSucursal")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCrecionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
                    b.HasOne("TotalHRInsight.DAL.Planilla", null)
                        .WithMany("Permisos")
                        .HasForeignKey("PlanillaIdPlanilla");

                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Planilla", b =>
                {
                    b.HasOne("TotalHRInsight.DAL.ApplicationUser", "UsuarioCreacion")
                        .WithMany()
                        .HasForeignKey("UsuarioCreacionId");

                    b.Navigation("UsuarioCreacion");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Pedido", b =>
                {
                    b.Navigation("PedidosProductos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Planilla", b =>
                {
                    b.Navigation("Asistencias");

                    b.Navigation("Permisos");
                });

            modelBuilder.Entity("TotalHRInsight.DAL.Producto", b =>
                {
                    b.Navigation("PedidosProductos");
                });
#pragma warning restore 612, 618
        }
    }
}
