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
        public int idAsistencia { get; set; }

        [Required]
        public DateTime FechaEntrada { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        [Required]
        public int UsuarioCrecionId { get; set; }

        public ApplicationUser? UsuarioCreacion { get; set; }

    }
}
