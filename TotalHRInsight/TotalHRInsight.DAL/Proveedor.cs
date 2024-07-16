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
    [Table("Proveedor")]
    public class Proveedor
    {
        [Key]
        public int IdProveedor { get; set; }

        [DisplayName("Proveedor")]
        [Required(ErrorMessage = "El nombre del proveedor es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre del proveedor no puede exceder los 100 caracteres")]
        public string NombreProveedor { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(150, ErrorMessage = "La descripcion no debe de exceder los 150")]
        public string Descripcion { get; set; }

        [DisplayName("Correo Electrónico")]
        [Required(ErrorMessage = "El correo electronico es requerido")]
        public string Email { get; set; }

        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El telefono es requerido")]
        public string Telefono { get; set; }

        [DisplayName("Método de Pago")]
        [Required(ErrorMessage = "El método de pago es requerido")]
        [DefaultValue(" ")]
        public string ? MetodoPago { get; set; }

        public ICollection<Producto> Producto { get; set; } = new List<Producto>();
    }
}
