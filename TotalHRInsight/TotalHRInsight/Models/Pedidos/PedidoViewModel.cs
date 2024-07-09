using TotalHRInsight.DAL;

namespace TotalHRInsight.Models.Pedidos
{
	public class PedidoViewModel
	{
		public Pedido Pedido { get; set; }
		public List<PedidosProductos> PedidosProductos { get; set; }
	}
}
