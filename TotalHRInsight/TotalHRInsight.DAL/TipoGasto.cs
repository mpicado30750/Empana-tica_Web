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
    [Table("TipoGasto")]
    public class TipoGasto
    {
        [Key]
        public int IdTipoGasto { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DisplayName("Tipo de Gasto")]
        public string NombreGasto { get; set; }

        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    }
}
