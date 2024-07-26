using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace TotalHRInsight.Models.Asistencia
{
    public class CrearAsistencia
    {
        public DateTime FechaEntrada { get; set; }

        public DateTime FechaSalida { get; set; }

        public int IdSucursal { get; set; }

        public string UsuarioCreacionId { get; set; }
    }
}
