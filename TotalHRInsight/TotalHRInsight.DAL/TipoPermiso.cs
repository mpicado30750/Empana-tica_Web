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
	[Table("TiposPermisos")]
	public class TipoPermiso
	{
		[Key]
		public int IdTipoPermiso { get; set; }

		[Required(ErrorMessage ="El campo es requerido")]
        [DisplayName("Tipo de Permiso")]
        public string NombrePermiso { get; set; }

	}
}
