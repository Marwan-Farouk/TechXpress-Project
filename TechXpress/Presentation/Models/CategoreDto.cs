using System.ComponentModel.DataAnnotations;

namespace BestStoreMVC.Models
{
    public class CategoreDto
    {

        [Required,MaxLength(100)]
        public string Name { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        [Required]
        public decimal Stock { get; set; }
    }
}
