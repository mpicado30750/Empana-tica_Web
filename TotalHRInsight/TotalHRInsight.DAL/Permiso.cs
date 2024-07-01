using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DefaultValue(false)]
        public bool Estado { get; set; }

        [DefaultValue("")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "El ID de la incidencia es obligatorio")]
        [ForeignKey("Incidencia")]
        public int IdIncidencia { get; set; }

		[Required(ErrorMessage = "El ID del usuario de creación es obligatorio")]
        [ForeignKey("UsuarioCreacion")]
        public string UsuarioCreacionId { get; set; }

		[Required(ErrorMessage = "El ID del usuario asignacion es obligatorio")]
        [ForeignKey("UsuarioAsignacion")]
        public string UsuarioAsignacionId { get; set; }

		public Incidencia? Incidencia { get; set; }

		public ApplicationUser? UsuarioCreacion { get; set; }

		public ApplicationUser? UsuarioAsignacion { get; set; }
	}
}
