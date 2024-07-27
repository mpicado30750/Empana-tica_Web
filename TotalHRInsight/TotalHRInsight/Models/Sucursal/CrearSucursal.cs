using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TotalHRInsight.Models.Sucursal
{
    public class CrearSucursal
    {
        [DisplayName("Sucursal")]
        [Required(ErrorMessage = "El nombre de la sucursal es obligatorio")]
        public string NombreSucursal { get; set; }
        
        [DisplayName("Ubicación")]
        [Required(ErrorMessage = "La ubicación de la sucursal es obligatoria")]
        public string UbicacionSucursal { get; set; }

        [DisplayName("Longitud")]
        [Required(ErrorMessage = "La Longitud de la sucursal es obligatoria")]
        public string Longitud { get; set; }

        [DisplayName("Latitud")]
        [Required(ErrorMessage = "La Latitud de la sucursal es obligatoria")]
        public string Latitud { get; set; }
    }
}
