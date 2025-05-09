using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.ViewModel.Admin
{
    public class UserRoleViewModel
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public List<SelectListItem> Users { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
    }
}
