
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
	public class ProductDetails
	{
		[Required, MaxLength(100)]
		public string Name { get; set; } = "";
		[Required]
		public decimal Price { get; set; }
		[Required]
		public string Description { get; set; } = "";
		[Required, MaxLength(100)]
		public string Category { get; set; } = "";
		[Required, MaxLength(100)]
		public string Brand { get; set; } = "";

		public IFormFile? ImageFile { get; set; }
	}
}
