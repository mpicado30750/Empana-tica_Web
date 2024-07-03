using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TotalHRInsight.DAL
{
	[Table("Incidencias")]
	public class Incidencia
	{
		[Key]
		public int IdIncidencia { get; set; }

		[Required(ErrorMessage ="El campo es requerido")]
        [DisplayName("Tipo de Incidencia")]
        public string NombreIncidencia { get; set; }

		public ICollection<Permiso> Permiso { get; set; } = new List<Permiso>();

	}
}
