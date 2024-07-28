using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TotalHRInsight.DAL
{
	[Table("AspNetUsers")]
	public class ApplicationUser : IdentityUser
	{
		[Required(ErrorMessage = "El nombre es obligatorio")]
		[MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
		public string Nombre { get; set; }

		[Required(ErrorMessage = "El primer apellido es obligatorio")]
		[MaxLength(100, ErrorMessage = "El primer apellido no puede exceder los 100 caracteres")]
		public string PrimerApellido { get; set; }

		[Required(ErrorMessage = "El segundo apellido es obligatorio")]
		[MaxLength(100, ErrorMessage = "El segundo apellido no puede exceder los 100 caracteres")]
		public string SegundoApellido { get; set; }

		[Required(ErrorMessage = "La fecha de nacimiento es obligatoria")]
		public DateTime FechaNacimiento { get; set; }

		[Required(ErrorMessage = "La fecha de registro es obligatoria")]
		public DateTime FechaRegistro { get; set; }

		[Phone(ErrorMessage = "El número de teléfono no es válido")]
		public int NumeroTelefono { get; set; }

		[Required(ErrorMessage = "El salario por hora es obligatorio")]
        public double Salario { get; set; }

		[Required]
		[DefaultValue(true)]
		public bool Estado { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [ForeignKey("Sucursal")]
        public int idSucursal { get; set; }

        public Sucursal? Sucursal { get; set; }

        public static implicit operator ApplicationUser(string v)
        {
            throw new NotImplementedException();
        }
    }

	//migrationBuilder.InsertData(
	//				table: "AspNetRoles",
 //                   columns: new[] { "Id", "Name", "NormalizedName" },
 //                   values: new object[,]
	//				{
	//					{ "S", "Supervisor", "Supervisor" },
	//					{ "A", "Administrador", "Administrador" },
	//					{ "U", "Usuario", "Usuario" }
	//				});

}
