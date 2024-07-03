using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("Planillas")]
	public class Planilla
	{
		[Key]
		public int IdPlanilla { get; set; }
        [DisplayName("Fecha de Inicio")]
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
		public DateTime FechaInicio { get; set; }

		[Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [DisplayName("Fecha de Fin")]
        public DateTime FechaFin { get; set; }

		[Required(ErrorMessage = "El monto total es obligatorio")]
		[Column(TypeName = "decimal(18,2)")]
        [DisplayName("Monto total")]
        public decimal MontoTotal { get; set; }
        [DisplayName("Asignado Por")]

        [Required(ErrorMessage = "El usuario de creación es obligatorio")]
		public string UsuarioCreacionId { get; set; }

		public int IdAsistencia { get; set; }

		public int IdPermiso { get; set; }

		[ForeignKey("UsuarioCreacionId")]
		public ApplicationUser? UsuarioCreacion { get; set; }

		public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();

		public ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
	}
}
