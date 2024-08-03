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

    [Table("TipoIngreso")]
    public class TipoIngreso
    {
        [Key]
        public int IdTipoIngreso { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DisplayName("Tipo de Ingreso")]
        public string NombreIngreso { get; set; }

        public ICollection<Ingreso> Ingreso { get; set; } = new List<Ingreso>();

    }
}
