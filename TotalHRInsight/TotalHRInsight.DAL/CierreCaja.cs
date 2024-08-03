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
    [Table("CierreCaja")]
    public class CierreCaja
    {
        [Key]
        public int IdCierraCaja { get; set; }

        [DisplayName("Fecha de Registro")]
        [Required(ErrorMessage = "La fecha de registro es obligatoria")]
        public DateTime Fecha { get; set; }

        [DisplayName("Sucursal")]
        [Required(ErrorMessage = "La Sucursal es obligatoria")]
        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; }

        [DisplayName("Monto Total")]
        [Required(ErrorMessage = "El monto es requerido")]
        [Column(TypeName = "double(18,2)")]
        public double MontoTotal { get; set; }
        public Sucursal? Sucursal { get; set; }

        public ICollection<Ingreso> Ingreso { get; set; } = new List<Ingreso>();
        public ICollection<Gasto> Gasto { get; set; } = new List<Gasto>();
    }
}
