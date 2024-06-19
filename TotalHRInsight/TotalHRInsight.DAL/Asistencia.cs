using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("Asistencia")]
    public class Asistencia
    {
        [Key]
        public int IdAsistencia { get; set; }

        
        public DateTime FechaEntrada { get; set; }

        
        public DateTime FechaSalida { get; set; }

        
        public string UbicacionEntrada { get; set; }
        
        public string UbicacionSalida { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
		[ForeignKey("UsuarioCreacion")]
		public string UsuarioCreacionId { get; set; }

        public ApplicationUser? UsuarioCreacion { get; set; }

    }
}
