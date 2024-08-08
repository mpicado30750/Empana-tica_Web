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

        [DisplayName("Fecha de Vencimiento")]
        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
		public DateTime FechaVencimiento { get; set; }

		[Required(ErrorMessage = "El precio unitario es obligatorio")]
        [DisplayName("Precio Unitario")]
        public double PrecioUnitario { get; set; }

		[DisplayName("Unidad de Medida")]
		[Required(ErrorMessage = "La unidad es obligatoria")]
		[ForeignKey("Medidas")]
		public int MedidasId { get; set; }

        [DisplayName("Categoría")]
        [Required(ErrorMessage = "La categoria es obligatoria")]
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }

        [DisplayName("Proveedor")]
        [Required(ErrorMessage = "El proveedor es obligatorio")]
        [ForeignKey("Proveedor")]
        public int ProveedorId { get; set; }
        public Medida? Medidas { get; set; }
        public Categoria? Categorias { get; set; }
        public Proveedor? Proveedor { get; set; }

        public ICollection<PedidosProductos> PedidosProductos { get; set; } = new List<PedidosProductos>();

        public ICollection<Inventario> Inventario { get; set; } = new List<Inventario>();

    }
}
