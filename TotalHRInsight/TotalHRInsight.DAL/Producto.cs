using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("Productos")]
	public class Producto
	{
		[Key]
		public int IdProducto { get; set; }

		[Required(ErrorMessage = "El nombre del producto es obligatorio")]
		[MaxLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres")]
		public string NombreProducto { get; set; }

		[Required(ErrorMessage = "La descripción es obligatoria")]
		[MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
		public string Descripcion { get; set; }

		[Required(ErrorMessage = "La cantidad disponible es obligatoria")]
		[Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa")]
		public int CantidadDisponible { get; set; }

		[Required(ErrorMessage = "La unidad es obligatoria")]
		[Range(0.01, double.MaxValue, ErrorMessage = "La unidad debe ser mayor a cero")]
		public float Unidad { get; set; }

		[Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
		public DateTime FechaVencimiento { get; set; }

		[Required(ErrorMessage = "El precio unitario es obligatorio")]
		[Column(TypeName = "decimal(18,2)")]
		public decimal PrecioUnitario { get; set; }

		public ICollection<PedidosProductos> PedidosProductos { get; set; } = new List<PedidosProductos>();
	}
}
