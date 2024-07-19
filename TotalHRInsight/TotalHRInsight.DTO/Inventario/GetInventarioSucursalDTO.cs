using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Inventario
{
    public class GetInventarioSucursalDTO
    {
        public int IdInventario { get; set; }
        public DateTime FechaVencimientoProducto { get; set; }
        public int DiasParaCaducar { get; set; }
        public int CantidadDisponible { get; set; }
        public double PrecioProducto { get; set; }
        public string NombreProducto { get; set; }
    }
}
