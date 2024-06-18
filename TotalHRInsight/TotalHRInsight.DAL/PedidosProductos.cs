using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("PedidoProductos")]
	public class PedidosProductos
	{
		[Key]
		public int PedidosProductosID { get; set; }

		[Required(ErrorMessage = "El ID del producto es obligatorio")]
		[ForeignKey("Producto")]
		public int ProductosID { get; set; }

		[Required(ErrorMessage = "El ID del pedido es obligatorio")]
		[ForeignKey("Pedido")]
		public int PedidoID { get; set; }

		[Required(ErrorMessage = "La cantidad es obligatoria")]
		[Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero")]
		public float Cantidad { get; set; }

		[Required(ErrorMessage = "La medida es obligatoria")]
		[MaxLength(50, ErrorMessage = "La medida no puede exceder los 50 caracteres")]
		public string Medida { get; set; }

		public Producto? Producto { get; set; }
		public Pedido? Pedido { get; set; }
	}
}
