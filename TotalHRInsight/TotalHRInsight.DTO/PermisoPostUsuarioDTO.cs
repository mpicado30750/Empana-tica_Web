using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO
{
    public class PermisoPostUsuarioDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Comentario { get; set; }
        public int CantidadDias { get; set; }
        public int IdTipoPermiso { get; set; }
        public int IdEstado { get; set; }
        public string UsuarioCreacionId { get; set; }
        public string UsuarioAsignacionId { get; set; }
    }
}
