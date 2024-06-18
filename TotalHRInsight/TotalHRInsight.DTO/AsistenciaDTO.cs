using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO
{
	public class AsistenciaDTO
	{
		public DateTime FechaEntrada { get; set; }

		public DateTime FechaSalida { get; set; }

		public int UsuarioCrecionId { get; set; }

	}
}
