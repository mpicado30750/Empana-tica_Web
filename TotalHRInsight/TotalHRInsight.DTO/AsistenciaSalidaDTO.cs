using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalHRInsight.DTO
{
	public class AsistenciaSalidaDTO
	{
        public int IdAsistencia { get; set; }

        public DateTime FechaSalida { get; set; }

		public string UbicacionSalida { get; set; }

		public string UsuarioCreacionId { get; set; }

	}
}
