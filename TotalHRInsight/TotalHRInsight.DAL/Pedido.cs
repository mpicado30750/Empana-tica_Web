using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        public int IdPedido { get; set; }

        [Required]
        public DateOnly FechaPedido { get; set; }

        [Required]
        public int CantidadPedido { get; set; }

        public float  MontoTotal {  get; set; } 
        
        public int UsuarioCrecionId {  get; set; }
        
        public int IdSucursal { get; set; }

        public Sucursal? Sucursal {  get; set; }
        public ApplicationUser? UsuarioCreacion { get; set; }
        public ICollection <Producto> Productos { get; set; } =new List<Producto> ();
    }
}
