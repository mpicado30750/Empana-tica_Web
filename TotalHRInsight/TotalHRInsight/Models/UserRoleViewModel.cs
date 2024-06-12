using TotalHRInsight.DAL;

namespace TotalHRInsight.Models
{
    public class UserRoleViewModel
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }
    }
}