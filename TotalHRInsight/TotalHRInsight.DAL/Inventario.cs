using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
    public class Inventario
    {
        [Key]
        public int IdInventario { get; set; }

        [ForeignKey("UsuarioCreacion")]
        [Required(ErrorMessage = "El usuario creacion es obligatorio")]
        [DisplayName("Creado Por")]
        public string UsuarioCreacionid { get; set; }
        public ApplicationUser? UsuarioCreacion { get; set; }

        [ForeignKey("UsuarioModificacion")]
        [Required(ErrorMessage = "El usuario modificación es obligatorio")]
        [DisplayName("Modificado Por")]
        public string UsuarioModificacionid { get; set; }
        public ApplicationUser? UsuarioModificacion { get; set; }

        [Required(ErrorMessage = "La fecha de creación del registro es obligatoria")]
        [DisplayName("Fecha de Registro")]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = "La fecha de modificación del registro es obligatoria")]
        [DisplayName("Fecha de Modificación")]
        public DateTime FechaModificacion { get; set; }

        [DisplayName("Cantidad Disponible")]
        [Required(ErrorMessage = "La cantidad disponible es obligatoria")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad disponible no puede ser negativa")]
        public int CantidadDisponible { get; set; }

        [DisplayName("Sucursal")]
        [Required(ErrorMessage = "El nombre de la sucursal es obligatorio")]
        [ForeignKey("Sucursal")]
        public int SucursalId { get; set; }
        public Sucursal? Sucursal { get; set; }

        [DisplayName("Producto")]
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [ForeignKey("Producto")]
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }

    }
}