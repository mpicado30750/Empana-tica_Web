﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("Sucursales")]
    public class Sucursal
    {
        [Key]
        public int IdSucursal { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string NombreSucursal { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string UbicacionSucursal { get; set; }
    }
}
