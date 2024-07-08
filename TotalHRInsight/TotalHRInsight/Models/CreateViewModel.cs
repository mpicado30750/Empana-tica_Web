using Microsoft.AspNetCore.Mvc.Rendering;
using TotalHRInsight.DAL;

namespace TotalHRInsight.Models
{
    public class CreateViewModel
    {
        public IEnumerable<Producto> Productos { get; set; }
        public SelectList Estados { get; set; }
        public SelectList Sucursales { get; set; }
        public SelectList Usuarios { get; set; }
    }
}
