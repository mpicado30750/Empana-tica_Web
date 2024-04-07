using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    public class ApplicationUser:IdentityUser
    {
        public string Nombre { get; set; }

        public string PrimwerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public DateOnly FechaNacimiento { get; set; }
        
        public DateTime FechaRegistro { get; set; }

        public float NumeroTelefono { get; set; }
        
        public decimal Salario { get; set; }
    }
}
