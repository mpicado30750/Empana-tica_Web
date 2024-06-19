using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO
{
	public class AsistenciaEntradaDTO
	{

		public DateTime FechaEntrada { get; set; }

		public string UbicacionEntrada { get; set; }

		public string UsuarioCreacionId { get; set; }

	}
}
