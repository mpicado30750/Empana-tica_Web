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

        public SelectList Roles { get; set; }  // This will populate the dropdown in the view
    }
}
