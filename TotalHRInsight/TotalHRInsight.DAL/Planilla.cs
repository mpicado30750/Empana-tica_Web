using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DAL
{
        [Table("Planillas")]
        public class Planilla
        {
            [Key]
            public int IdPlanilla { get; set; }

            [Required]
            public DateOnly FechaInicio { get; set; }

            [Required]
            public DateOnly FechaFin { get; set; }
            
            [Required]
            public float MontoTotal { get; set; }

            [Required]
            public int UsuarioCrecionId { get; set; }
            public int IdAsistencia { get; set; }
            public int IdPermiso { get; set; }
            public ApplicationUser? UsuarioCreacion { get; set; }
            public ICollection<Asistencia> Asistencias { get; set; } = new List<Asistencia>();
            public ICollection<Permiso> Permisos { get; set; } = new List<Permiso>();
        }
    }
