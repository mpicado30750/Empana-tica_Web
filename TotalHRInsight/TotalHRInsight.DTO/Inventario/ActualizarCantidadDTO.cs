using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Inventario
{
    public class ActualizarCantidadDTO
    {
        public string UsuarioModificacionid {  get; set; }
        public int Id { get; set; }
        public int NuevaCantidad { get; set; }
    }

}
