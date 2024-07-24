using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Pedidos
{
    public class ProductoViewModel
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int MedidasId { get; set; }
        public int CategoriaId { get; set; }
        public int ProveedorId { get; set; }
    }
}
