using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Productos
{
    public class ProductoVencimientoDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int DiasParaVencer { get; set; }
        public int CantidadDisponible { get; set; }
        public string NombreSucursal { get; set; }
    }
}
