using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    public class Estado
    {
        [Key]
        public int IdEstado { get; set; }

        [Required(ErrorMessage = "El espacio estado es obligatorio")]
        [DisplayName("Estado")]
        public string EstadoSolicitud { get; set; }
    }
}
