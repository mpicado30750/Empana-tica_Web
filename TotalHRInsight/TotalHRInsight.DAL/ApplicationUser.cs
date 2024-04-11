using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string PrimwerApellido { get; set; }

        [Required]
        [MaxLength(100)]
        public string SegundoApellido { get; set; }

        [Required]
        public DateOnly FechaNacimiento { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; }

        public float NumeroTelefono { get; set; }

        [Required]
        public float Salario { get; set; }
        [Column(TypeName = "decimal(18,4)")]

        [Required]
        [DefaultValue(1)]
        public int Estado {  get; set; }
    }
}
