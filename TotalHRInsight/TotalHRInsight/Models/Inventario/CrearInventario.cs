using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Models.Inventario
{
    public class CrearInventario
    {
        public int CantidadDisponible { get; set; }

        public int SucursalId { get; set; }

        public int ProductoId { get; set; }
    }
}
