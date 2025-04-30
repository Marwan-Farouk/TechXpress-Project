namespace Presentation.ViewModel.Admin
{
    public class UserRolesViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; }
    }
}
