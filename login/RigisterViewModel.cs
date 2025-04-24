using System;
using System.ComponentModel.DataAnnotations;

namespace RegisterProject.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [MinLength(6, ErrorMessage = "Username must be at least 6 characters.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        [RegularExpression("^(male|female)$", ErrorMessage = "Please select a valid gender.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Birthday is required.")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
    }
}