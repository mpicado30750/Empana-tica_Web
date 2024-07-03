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
        [DisplayName("Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
		public DateTime FechaInicio { get; set; }
        [DisplayName("Fecha de Fin")]

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
		public DateTime FechaFin { get; set; }
        [DisplayName("Dias de Permiso")]
        [Required(ErrorMessage = "La cantidad de días es obligatoria")]
		[Range(1, int.MaxValue, ErrorMessage = "La cantidad de días debe ser mayor a cero")]
		public int CantidadDias { get; set; }
        [DisplayName("Estado del Permiso")]
        [Required(ErrorMessage = "El estado es obligatorio")]
        [DefaultValue(false)]
        public bool Estado { get; set; }

        [DefaultValue("")]
        [DisplayName("Comentarios")]
        public string Comentario { get; set; }

        [Required(ErrorMessage = "El ID de la incidencia es obligatorio")]
        [ForeignKey("Incidencia")]
        public int IdIncidencia { get; set; }
        [DisplayName("Aprobado Por")]
        [Required(ErrorMessage = "El ID del usuario de creación es obligatorio")]
        [ForeignKey("UsuarioCreacion")]
        public string UsuarioCreacionId { get; set; }

        [DisplayName("Asignado a")]
        [Required(ErrorMessage = "El ID del usuario asignacion es obligatorio")]
        [ForeignKey("UsuarioAsignacion")]
        public string UsuarioAsignacionId { get; set; }

		public Incidencia? Incidencia { get; set; }

		public ApplicationUser? UsuarioCreacion { get; set; }

		public ApplicationUser? UsuarioAsignacion { get; set; }
	}
}
