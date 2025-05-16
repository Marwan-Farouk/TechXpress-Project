using System.ComponentModel.DataAnnotations;

namespace Presentation.ActionRequests.User;

public class RegisterActionRequest
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
    [Required]
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
    [Required]
    [Compare("PhoneNumber")]
    public string PhoneNumberConfirm { get; set; }
    public List<AddAddressActionRequest> Addresses { get; set; }
    [Required]
    public string Role { get;set; }
}



