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
    [Table("Categoria")]
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [DisplayName("Categoría")]
        [Required(ErrorMessage = "La categoria es requerida")]
        [MaxLength(100, ErrorMessage = "La categoria no puede exceder los 100 caracteres")]
        public string NombreCategoria { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(150, ErrorMessage = "La descripcion no debe de exceder los 150")]
        public string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
