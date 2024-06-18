using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("Sucursales")]
	public class Sucursal
	{
		[Key]
		public int IdSucursal { get; set; }

		[Required(ErrorMessage = "El nombre de la sucursal es obligatorio")]
		[MaxLength(100, ErrorMessage = "El nombre de la sucursal no puede exceder los 100 caracteres")]
		public string NombreSucursal { get; set; }

		[Required(ErrorMessage = "La ubicación de la sucursal es obligatoria")]
		public string UbicacionSucursal { get; set; }

		[Required(ErrorMessage = "Este campo es requerido")]
		public double Longitud { get; set; }

		[Required(ErrorMessage = "Este campo es requerido")]
		public double Latitud { get; set; }
	}
}
