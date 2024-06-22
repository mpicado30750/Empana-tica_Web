using TotalHRInsight.DAL;

namespace TotalHRInsight.Models
{
    public class AsistenciaModel
    {
        public int Id { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public double LatitudEntrada { get; set; }
        public double LongitudEntrada { get; set; }
        public double? LatitudSalida { get; set; }
        public double? LongitudSalida { get; set; }
        public string UsuarioCreacionId { get; set; }
        public string UsuarioCreacion { get; set; }
    }
}
