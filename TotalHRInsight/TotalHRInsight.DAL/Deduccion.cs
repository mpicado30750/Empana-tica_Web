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
    [Table("Deduccion")]
    public class Deduccion
    {
        [Key]
        public int IdDeduccion { get; set; }

        [DisplayName("Fecha de Registro")]
        [Required(ErrorMessage = "La fecha de registro es obligatoria")]
        public DateTime FechaDeduccion { get; set; }

        [DisplayName("Deducción")]
        [Required(ErrorMessage = "La deducción es requerida")]
        [MaxLength(100, ErrorMessage = "El nombre de deducción no puede exceder los 100 caracteres")]
        public string NombreDeduccion { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(150, ErrorMessage = "La descripcion no debe de exceder los 150")]
        public string Descripcion { get; set; }

        [DisplayName("Monto de Deducción")]
        [Required(ErrorMessage = "El monto es requerido")]
        public double MontoDeduccion { get; set; }

        [DisplayName("Asignado Por")]
        [Required(ErrorMessage = "El ID del usuario creacion es obligatorio")]
        [ForeignKey("UsuarioCreacion")]
        public string UsuarioCreacionId { get; set; }

        [DisplayName("Asignado a")]
        [Required(ErrorMessage = "El ID del usuario asignacion es obligatorio")]
        [ForeignKey("UsuarioAsignacion")]
        public string UsuarioAsignacionId { get; set; }

        [DisplayName("Salario")]
        [Required(ErrorMessage = "El salario es obligatorio")]
        [ForeignKey("Salario")]
        public int SalarioId { get; set; }

        public ApplicationUser? UsuarioCreacion { get; set; }

        public ApplicationUser? UsuarioAsignacion { get; set; }

        public Salario? Salario { get; set; }


        [DisplayName("Tipo de Deduccion")]
        [Required(ErrorMessage = "El tipo de deduccion es obligatorio")]
        [ForeignKey("TipoDeduccion")]
        public int TipoDeduccionId { get; set; }


        public TipoDeduccion? TipoDeduccion { get; set; }
    }
}
