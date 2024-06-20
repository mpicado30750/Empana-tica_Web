using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("Permisos")]
	public class Permiso
	{
		[Key]
		public int IdPermisos { get; set; }

		[Required(ErrorMessage = "La fecha de inicio es obligatoria")]
		public DateTime FechaInicio { get; set; }

		[Required(ErrorMessage = "La fecha de fin es obligatoria")]
		public DateTime FechaFin { get; set; }

		[Required(ErrorMessage = "La cantidad de días es obligatoria")]
		[Range(1, int.MaxValue, ErrorMessage = "La cantidad de días debe ser mayor a cero")]
		public int CantidadDias { get; set; }

		[Required(ErrorMessage = "El estado es obligatorio")]
		public bool Estado { get; set; }

		[Required(ErrorMessage = "El ID de la incidencia es obligatorio")]
		public int IdIncidencia { get; set; }

		[Required(ErrorMessage = "El ID del usuario de creación es obligatorio")]
		public string UsuarioCreacionId { get; set; }

		[Required(ErrorMessage = "El ID del usuario asignacion es obligatorio")]
		public string UsuarioAsignacionId { get; set; }

		[ForeignKey("IdIncidencia")]
		public Incidencia? Incidencia { get; set; }

		[ForeignKey("UsuarioCreacionId")]
		public ApplicationUser? UsuarioCreacion { get; set; }

		[ForeignKey("UsuarioAsignacionId")]
		public ApplicationUser? UsuarioAsignacion { get; set; }
	}
}
