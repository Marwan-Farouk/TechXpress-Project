using System.ComponentModel.DataAnnotations;

namespace Presentation.ActionRequests.User;

public class CreateRoleActionRequest
{
    [Required]
    public string Name { get; set; }

}