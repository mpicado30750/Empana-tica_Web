using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("Producto")]
	public class Producto
	{
		[Key]
		public int IdProducto { get; set; }

        [DisplayName("Producto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
		[MaxLength(100, ErrorMessage = "El nombre del producto no puede exceder los 100 caracteres")]
		public string NombreProducto { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es obligatoria")]
		[MaxLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres")]
		public string Descripcion { get; set; }


        [DisplayName("Unidad de Medida")]
        [Required(ErrorMessage = "La unidad es obligatoria")]
		public string Unidad { get; set; }

        [DisplayName("Fecha de Vencimiento")]
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
		public DateTime FechaVencimiento { get; set; }

		[Required(ErrorMessage = "El precio unitario es obligatorio")]
		[Column(TypeName = "decimal(18,2)")]
        [DisplayName("Precio Unitario")]
        public double PrecioUnitario { get; set; }

        public Medida? Medidas { get; set; }

        public ICollection<PedidosProductos> PedidosProductos { get; set; } = new List<PedidosProductos>();

        public ICollection<Inventario> Inventario { get; set; } = new List<Inventario>();
    }
}
