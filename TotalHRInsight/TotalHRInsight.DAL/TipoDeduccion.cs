﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("TipoDeduccion")]
    public class TipoDeduccion
    {
        [Key]
        public int IdTipoDeduccion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        [DisplayName("Tipo de Deduccion")]
        public string NombreDeduccion { get; set; }

        public ICollection<Deduccion> Deduccion { get; set; } = new List<Deduccion>();

    }
}
