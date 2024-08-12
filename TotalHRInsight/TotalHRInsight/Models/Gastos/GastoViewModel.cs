using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Models.Gastos
{
    public class GastoViewModel
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
        public double MontoGasto { get; set; }

        [DisplayName("Cierre de Caja")]
        [Required(ErrorMessage = "El cierre de caja es obligatorio")]
        [ForeignKey("CierreCaja")]
        public int CierreId { get; set; }
        public CierreCaja? CierreCaja { get; set; }

        [DisplayName("Sucursal")]
        [Required(ErrorMessage = "La sucursal es obligatoria")]
        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; }
        public TotalHRInsight.DAL.Sucursal? Sucursal { get; set; }

        public TipoGasto? TipoGasto { get; set; }
    }
}
