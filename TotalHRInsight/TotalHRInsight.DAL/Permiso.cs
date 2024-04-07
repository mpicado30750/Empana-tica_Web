using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
        [Table("Permisos")]
        public class Permiso
        {
            [Key]
            public int idPermisos { get; set; }

            [Required]
            public DateOnly FechaInicio { get; set; }

            [Required]
            public DateOnly FechaFin { get; set; }

            [Required]
            public string TipoPermiso { get; set; }

            [Required]
            public int CantidadDias {  get; set; }

            [Required]
            public bool Estado {  get; set; }   

            [Required]
            public int UsuarioCrecionId { get; set; }

            public ApplicationUser? UsuarioCreacion { get; set; }

        }
    }