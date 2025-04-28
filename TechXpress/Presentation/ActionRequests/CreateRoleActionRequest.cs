using System.ComponentModel.DataAnnotations;

namespace Presentation.ActionRequests;

public class CreateRoleActionRequest
{
    [Required]
    public string Name { get; set; }

}