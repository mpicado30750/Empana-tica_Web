﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
    [Table("Asistencia")]
    public class Asistencia
    {
        [Key]
        public int idAsistencia { get; set; }

        [Required(ErrorMessage ="Se debe de ingresar una fecha y hora válida")]
        public DateTime FechaEntrada { get; set; }

        [Required(ErrorMessage = "Se debe de ingresar una fecha y hora válida")]
        public DateTime FechaSalida { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public double Longitud {  get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public double Latitud { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Ubicacion { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
		[ForeignKey("UsuarioCreacion")]
		public string UsuarioCreacionId { get; set; }

        public ApplicationUser? UsuarioCreacion { get; set; }

    }
}
