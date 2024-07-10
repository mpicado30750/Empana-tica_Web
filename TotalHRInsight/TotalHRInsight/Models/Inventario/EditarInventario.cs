namespace TotalHRInsight.Models.Inventario
{
    public class EditarInventario
    {
        public int IdInventario { get; set; }
        public int ProductoId { get; set; }
        public int SucursalId { get; set; }
        public int CantidadDisponible { get; set; }
    }
}
