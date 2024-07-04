using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Fecha y Hora de Entrada")]
        public DateTime FechaEntrada { get; set; }

        [DisplayName("Fecha y Hora de Salida")]
        public DateTime FechaSalida { get; set; }
        [DisplayName("Ubicación de Entrada")]
        public string UbicacionEntrada { get; set; }
        [DisplayName("Ubicación de Salida")]
        public string UbicacionSalida { get; set; }

        [DisplayName("Asignado Por")]
        [Required(ErrorMessage = "Este campo es requerido")]
		[ForeignKey("UsuarioCreacion")]
		public string UsuarioCreacionId { get; set; }

        public ApplicationUser? UsuarioCreacion { get; set; }

    }
}
