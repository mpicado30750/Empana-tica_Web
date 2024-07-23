using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Inventario
{
    public class DevolverProductosDTO
    {
        public DateTime Fecha {  get; set; }
        public string idUsuario { get; set; }
        public List<ListaProductoDevolver> listaProductoDevolvers { get; set; }
    }

    public class ListaProductoDevolver
    {
        public int IdInventario { get; set; }
        public string nombreProducto { get; set; }
        public int cantidadDisponible { get; set; }

    }
}
