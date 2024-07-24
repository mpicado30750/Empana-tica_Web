using Microsoft.EntityFrameworkCore;

namespace TotalHRInsight.DAL
{
    public class TotalHRInsightDbContext : DbContext
    {
        public TotalHRInsightDbContext() { }
        public TotalHRInsightDbContext(DbContextOptions<TotalHRInsightDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=viaduct.proxy.rlwy.net;Port=24583;Database=railway;User=root;Password=ZkYGcAFDHXksHyHUzxQDjGmVmqJyJUuK;";

            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<Sucursal> Sucursales { get; set; }
        public virtual DbSet<Planilla> Planillas { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<Asistencia> Asistencias { get; set; }
        public virtual DbSet<PedidosProductos> PedidosProductos { get; set; }
        public virtual DbSet<TipoPermiso> TipoPermisos { get; set; }
        public virtual DbSet<Estado> Estados { get; set; }
        public virtual DbSet<Medida> Medidas { get; set; }
        public virtual DbSet<Inventario> Inventario { get; set; }
        public virtual DbSet<Proveedor> Proveedor { get; set; }
        public virtual DbSet<Categoria> Categoria { get; set; }
		public virtual DbSet<TipoDeduccion> TipoDeduccions { get; set; }
		public virtual DbSet<Deduccion> Deduccions { get; set; }
		public virtual DbSet<Salario> Salarios { get; set; }
	}
}
