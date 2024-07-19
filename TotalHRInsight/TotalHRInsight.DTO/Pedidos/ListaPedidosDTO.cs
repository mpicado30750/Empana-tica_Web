using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO.Pedidos
{
    public class ListaPedidosDTO
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public DateTime FechaEntrega { get; set; }
        public string UsuarioCreacionId { get; set; }
        public int IdSucursal { get; set; }
        public int IdEstado { get; set; }
        public string sucursal { get; set; }
        public string estado { get; set; }
        public double MontoTotal { get; set; }
    }
}
