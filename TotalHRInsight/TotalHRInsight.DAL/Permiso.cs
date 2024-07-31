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
        
        [DefaultValue("")]
        [DisplayName("Comentarios")]
        [Required(ErrorMessage = "Los comentarios son requeridos")]
        public string Comentario { get; set; }

        [DefaultValue("")]
        [DisplayName("Motivo de Solicitud")]
        public string MotivoAdmin { get; set; }

        [Required(ErrorMessage = "El tipo de permiso es obligatorio")]
        [ForeignKey("TipoPermisos")]
        public int IdTipoPermiso { get; set; }

        [DisplayName("Aprobado Por")]
        [Required(ErrorMessage = "El ID del usuario de creación es obligatorio")]
        [ForeignKey("UsuarioCreacion")]
        public string UsuarioCreacionId { get; set; }

        [DisplayName("Asignado a")]
        [Required(ErrorMessage = "El ID del usuario asignacion es obligatorio")]
        [ForeignKey("UsuarioAsignacion")]
        public string UsuarioAsignacionId { get; set; }

        [Required(ErrorMessage = "El Estado de solicitud es obligatorio")]
        [ForeignKey("Estado")]
        public int IdEstado { get; set; }

        public TipoPermiso? TipoPermisos { get; set; }

		public ApplicationUser? UsuarioCreacion { get; set; }

		public ApplicationUser? UsuarioAsignacion { get; set; }
        public Estado? Estado { get; set; }
    }
}
