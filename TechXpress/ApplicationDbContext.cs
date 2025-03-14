using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SPresentation.Models;

namespace Presentation.Services
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		public DbSet<Product> Products { get; set; }
	}
}
