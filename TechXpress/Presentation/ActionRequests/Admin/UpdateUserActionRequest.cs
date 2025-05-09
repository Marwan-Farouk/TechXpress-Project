using System.ComponentModel.DataAnnotations;

namespace Presentation.ActionRequests.Admin
{
    public class UpdateUserActionRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public DateTime DateCreated { get; set; }
    
    }
}
