using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Models.Planilla
{
    public class CrearPlanilla
    {
        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria")]
        [Display(Name = "Fecha de Fin")]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(150, ErrorMessage = "La descripcion no debe de exceder los 150")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El usuario de asignacion es obligatorio")]
        [Display(Name = "Asignado a")]
        public string UsuarioAsignacionId { get; set; }

        public string CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }
    }
}
