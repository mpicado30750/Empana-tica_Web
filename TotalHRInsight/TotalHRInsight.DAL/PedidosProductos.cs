using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("PedidoProductos")]
    public class PedidosProductos
    {
        [Key]
        public int PedidosProductosID { get; set;}

        [Required]
        [ForeignKey("Producto")]
        public int ProductosID { get; set;}

        [Required]
        [ForeignKey("Pedido")]
        public int PedidoID {  get; set;}

        [Required]
        public float Cantidad {  get; set;}

        [Required]
        public string Medida { get; set;}

        public Producto ? Producto {  get; set;}

        public Pedido ? Pedido { get; set;}
    }
}
