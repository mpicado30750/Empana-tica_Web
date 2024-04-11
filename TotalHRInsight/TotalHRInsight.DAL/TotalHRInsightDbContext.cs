using Microsoft.EntityFrameworkCore;

namespace TotalHRInsight.DAL
{
    public class TotalHRInsightDbContext : DbContext
    {
        public TotalHRInsightDbContext() { }
        public TotalHRInsightDbContext(DbContextOptions<TotalHRInsightDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer();
        }
        public virtual DbSet<Sucursal> Sucursales { get; set; }
        public virtual DbSet<Planilla> Planillas { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<Asistencia> Asistencias { get; set; }


    }
}
