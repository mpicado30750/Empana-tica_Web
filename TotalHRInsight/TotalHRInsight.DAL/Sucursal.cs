using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("Sucursales")]
	public class Sucursal
	{
		[Key]
		public int IdSucursal { get; set; }
        [DisplayName("Sucursal")]
        [Required(ErrorMessage = "El nombre de la sucursal es obligatorio")]
		[MaxLength(100, ErrorMessage = "El nombre de la sucursal no puede exceder los 100 caracteres")]
		public string NombreSucursal { get; set; }
        [DisplayName("Ubicación")]
        [Required(ErrorMessage = "La ubicación de la sucursal es obligatoria")]
		public string UbicacionSucursal { get; set; }
        [DisplayName("Longitud")]
		[Required(ErrorMessage = "Este campo es requerido")]
		public double Longitud { get; set; }
        [DisplayName("Latitud")]
        [Required(ErrorMessage = "Este campo es requerido")]
		public double Latitud { get; set; }
	}
}
