using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Models.Gastos
{
    public class IngresoViewModel
    {
        public int IdIngreso { get; set; }

        [DisplayName("Fecha de Registro")]
        [Required(ErrorMessage = "La fecha de registro es obligatoria")]
        public DateTime Fecha { get; set; }

        [DisplayName("Tipo de Ingreso")]
        [Required(ErrorMessage = "El tipo de Ingreso es obligatorio")]
        [ForeignKey("TipoIngreso")]
        public int TipoIngresoId { get; set; }

        [DisplayName("Monto de Ingreso")]
        [Required(ErrorMessage = "El monto es requerido")]
        [Column(TypeName = "double(18,2)")]
        public double MontoIngreso { get; set; }

        [DisplayName("Cierre de Caja")]
        [Required(ErrorMessage = "El cierre obligatorio")]
        [ForeignKey("CierreCaja")]
        public int CierreId { get; set; }
        public CierreCaja? CierreCaja { get; set; }
        public TipoIngreso? TipoIngreso { get; set; }

        [DisplayName("Sucursal")]
        [Required(ErrorMessage = "La sucursal es obligatoria")]
        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; }
        public TotalHRInsight.DAL.Sucursal? Sucursal { get; set; }
    }
}
