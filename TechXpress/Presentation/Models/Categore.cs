using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BestStoreMVC.Models
{
    public class Categore
    {
        
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Stock { get; set; }

    }
}
