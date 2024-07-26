using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace TotalHRInsight.Models
{
    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }  // User ID is needed to identify the user

        [Required(ErrorMessage = "Please select a role")]
        [Display(Name = "Role")]
        public string SelectedRoleId { get; set; }  // This will hold the selected role ID from the dropdown

        public int idSucursal { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string PrimerApellido { get; set; }

        [Required]
        [MaxLength(100)]
        public string SegundoApellido { get; set; }

        [Required]
        public double Salario { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required]
        public int NumeroTelefono { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
