using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TotalHRInsight.Models.Deduccion
{
    public class CreacionDeduccion
    {
        public DateTime FechaDeduccion { get; set; }
        public string NombreDeduccion { get; set; }
        public double MontoDeduccion { get; set; }
        public string CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }
        public string UsuarioAsignacionId { get; set; }
        public int TipoDeduccionId { get; set; }
    }
}
