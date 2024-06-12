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
        [ForeignKey("UsuarioCreacion")]
        public string UsuarioCrecionId {  get; set; }

        [Required]
        [ForeignKey("Sucursal")]
        public int IdSucursal { get; set; }

        [Required]
        public string EstadoPedido { get; set; }

        [Required]
        public float MontoTotal { get; set; }

        public Sucursal? Sucursal {  get; set; }
        public ApplicationUser? UsuarioCreacion { get; set; }
        public ICollection <PedidosProductos> PedidosProductos { get; set; } =new List<PedidosProductos> ();
    }
}
