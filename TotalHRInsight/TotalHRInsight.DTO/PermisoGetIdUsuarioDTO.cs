using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO
{
    public class PermisoGetIdUsuarioDTO
    {
        public int IdPermisos { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Comentario { get; set; }
        public int IdIncidencia { get; set; }
        public int CantidadDias { get; set; }
        public bool Estado { get; set; }
        public string UsuarioCreacionId { get; set; }
    }
}
