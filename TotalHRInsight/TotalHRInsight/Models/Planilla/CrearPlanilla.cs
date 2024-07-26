using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Models.Planilla
{
    public class CrearPlanilla
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string Descripcion { get; set; }

        public double MontoTotal { get; set; }
        public string CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }
        public string UsuarioAsignacionId { get; set; }
    }
}
