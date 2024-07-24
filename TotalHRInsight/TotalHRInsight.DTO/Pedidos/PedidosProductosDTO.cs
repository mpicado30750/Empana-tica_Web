using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Pedidos
{
    public class PedidosProductosDTO
    {
        public DateTime Fecha { get; set; }
        public string idUsuario { get; set; }
        public int idSucursal { get; set; }
        public double total { get; set; }
        public List<ListaProductoPedido> listaProductoDevolvers { get; set; }
    }

    public class ListaProductoPedido
    {
        public int IdInventario { get; set; }
        public string nombreProducto { get; set; }
        public int cantidadDisponible { get; set; }

    }
}
