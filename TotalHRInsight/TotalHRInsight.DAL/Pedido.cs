using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("Pedidos")]
	public class Pedido
	{
		[Key]
		public int IdPedido { get; set; }

		[Required(ErrorMessage = "La fecha del pedido es obligatoria")]
		public DateTime FechaPedido { get; set; }

		[Required(ErrorMessage = "El usuario de creación es obligatorio")]
		[ForeignKey("UsuarioCreacion")]
		public string UsuarioCreacionId { get; set; }

		[Required(ErrorMessage = "La sucursal es obligatoria")]
		[ForeignKey("Sucursal")]
		public int IdSucursal { get; set; }

		[Required(ErrorMessage = "El estado del pedido es obligatorio")]
		[MaxLength(50, ErrorMessage = "El estado del pedido no puede exceder los 50 caracteres")]
		public string EstadoPedido { get; set; }

		[Required(ErrorMessage = "El monto total es obligatorio")]
		[Column(TypeName = "double(18,2)")]
		public double MontoTotal { get; set; }

		public Sucursal? Sucursal { get; set; }
		public ApplicationUser? UsuarioCreacion { get; set; }
		public ICollection<PedidosProductos> PedidosProductos { get; set; } = new List<PedidosProductos>();
	}
}
