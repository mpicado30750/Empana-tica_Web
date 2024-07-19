using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("Salario")]
    public class Salario
    {
        [Key]
        public int IdSalario { get; set; }

        public double SalarioBruto { get; set; }

        public double SalarioExtra { get; set; }

        public double SalarioNeto { get; set; }

        [DisplayName("Asignado Por")]
        [Required(ErrorMessage = "El ID del usuario asignacion es obligatorio")]
        [ForeignKey("UsuarioCreacion")]
        public string UsuarioCreacionId { get; set; }

        [DisplayName("Asignado a")]
        [Required(ErrorMessage = "El ID del usuario asignacion es obligatorio")]
        [ForeignKey("UsuarioAsignacion")]
        public string UsuarioAsignacionId { get; set; } 

        public ApplicationUser? UsuarioCreacion { get; set; }

        public ApplicationUser? UsuarioAsignacion { get; set; }

        public ICollection<Deduccion> Deduccion { get; set; } = new List<Deduccion>();
    }
}
