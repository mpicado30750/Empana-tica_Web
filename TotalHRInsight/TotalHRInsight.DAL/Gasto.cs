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
    [Table("Gasto")]
    public class Gasto
    {
        [Key]
        public int IdGasto { get; set; }

        [DisplayName("Fecha de Registro")]
        [Required(ErrorMessage = "La fecha de registro es obligatoria")]
        public DateTime Fecha { get; set; }

        [DisplayName("Tipo de Gasto")]
        [Required(ErrorMessage = "El tipo de gasto es obligatorio")]
        [ForeignKey("TipoGasto")]
        public int TipoGastoId { get; set; }

        [DisplayName("Monto de Gasto")]
        [Required(ErrorMessage = "El monto es requerido")]
        [Column(TypeName = "double(18,2)")]
        public double MontoGasto{ get; set; }

        [DisplayName("Cierre de Caja")]
        [Required(ErrorMessage = "El cierre obligatorio")]
        [ForeignKey("CierreCaja")]
        public int CierreId { get; set; }
        public CierreCaja? CierreCaja { get; set; }
        public TipoGasto? TipoGasto { get; set; }
    }
}
