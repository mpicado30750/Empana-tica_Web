using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Constraints;

namespace TotalHRInsight.DAL
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public int CantidadDisponible { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public float Unidad { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public DateOnly FechaVencimiento { get; set; }

        [Required(ErrorMessage ="Este campo es requerido")]
        public float PrecioUnitario { get; set; }

        public ICollection<PedidosProductos> PedidosProductos { get; set; } = new List<PedidosProductos>();
    }
}
