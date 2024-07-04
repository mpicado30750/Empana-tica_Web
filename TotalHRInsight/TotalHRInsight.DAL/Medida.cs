using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    public class Medida
    {
        [Key]
        public int IdMedida { get; set; }

        [Required(ErrorMessage = "El espacio es requerido")]
        [DisplayName("Unidad de Medida")]
        public string NombreMedida { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
