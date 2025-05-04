using System.ComponentModel.DataAnnotations;

namespace Presentation.ActionRequests.User;

public class LoginActionRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool RememberMe { get; set; }
}