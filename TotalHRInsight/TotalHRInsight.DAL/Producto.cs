using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        public string NombreProducto { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public int CantidadDisponible { get; set; }

        [Required]
        public DateOnly FechaVencimiento { get; set; }

        [Required]
        public float PrecioUnitario { get; set; }

        public ICollection<PedidosProductos> PedidosProductos { get; set; } = new List<PedidosProductos>();
    }
}
