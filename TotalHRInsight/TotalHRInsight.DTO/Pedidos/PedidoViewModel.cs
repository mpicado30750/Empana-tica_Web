using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Pedidos
{
    public class PedidoViewModel
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string UsuarioCreacionId { get; set; }
        public int IdSucursal { get; set; }
        public string IdEstado { get; set; }
        public double MontoTotal { get; set; }
        public ICollection<PedidoProductoViewModel> PedidosProductos { get; set; }
    }

    public class PedidoProductoViewModel
    {
        public int PedidosProductosID { get; set; }
        public int ProductosID { get; set; }
        public int PedidoID { get; set; }
        public int Cantidad { get; set; }
        public ProductoViewModel Producto { get; set; }
    }

}
