using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccess.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<UserPhone> UserPhones { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=DESKTOP-7VF6E7V;Database=TechXpress_DB;Integrated Security = SSPI; TrustServerCertificate = True";
                optionsBuilder.UseSqlServer(connectionString).LogTo(Console.WriteLine, LogLevel.Information);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // [1] User
            //=================
            // Table name
            modelBuilder.Entity<User>().ToTable("Users");
            // Primary key
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            // Column Data Types
            modelBuilder.Entity<User>().Property(u => u.Id) // ID
                .HasColumnType("int").UseIdentityColumn(100, 1);
            modelBuilder.Entity<User>().Property(u => u.FirstName) // FirstName
                .HasColumnType("nvarchar(max)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.LastName) // LastName
                .HasColumnType("nvarchar(max)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Email) // Email
                .HasColumnType("nvarchar(max)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Password) // Password
                .HasColumnType("nvarchar(max)").IsRequired();
            modelBuilder.Entity<User>().Property(u => u.BirthDate) // BirthDate
                .HasColumnType("datetime");
            modelBuilder.Entity<User>().Property(u => u.DateCreated) // DateCreated
                .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>().Property(u => u.Role) // Role
                .HasColumnType("nvarchar(max)").HasDefaultValue("customer");
            // Relationships

            // UserPhone (many) ===> User (one)
            modelBuilder.Entity<User>()
                .HasMany(u => u.userPhones)
                .WithOne(up => up.User);

            // UserAddress (many) ===> User (one)
            modelBuilder.Entity<User>()
                .HasMany(u => u.userAddresses)
                .WithOne(ua => ua.User);
            //====================================================================================================
            // [2] UserPhone
            //=================
            // Table name
            modelBuilder.Entity<UserPhone>().ToTable("UserPhones");
            // Primary key
            modelBuilder.Entity<UserPhone>().HasKey(up => new { up.UserId, up.PhoneNumber });
            // Column Data Types
            modelBuilder.Entity<UserPhone>().Property(up => up.UserId).HasColumnType("int"); // FK userId
            modelBuilder.Entity<UserPhone>().Property(up => up.PhoneNumber).HasColumnType("nvarchar(11)"); // phoneNumber
            //====================================================================================================
            // [3] UserAddress
            //=================
            // Table name
            modelBuilder.Entity<UserAddress>().ToTable("UserAddresses");
            // Primary key
            modelBuilder.Entity<UserAddress>().HasKey(ua => new { ua.UserId, ua.AddressId });
            // Column Data Types
            modelBuilder.Entity<UserAddress>().Property(ua => ua.UserId) // FK userId
                .HasColumnType("int"); 
            modelBuilder.Entity<UserAddress>().Property(ua => ua.AddressId) // addressId
                .HasColumnType("int"); 
            modelBuilder.Entity<UserAddress>().Property(ua => ua.Country) // country
                .HasColumnType("nvarchar(max)").IsRequired(); 
            modelBuilder.Entity<UserAddress>().Property(ua => ua.City) // city
                .HasColumnType("nvarchar(max)").IsRequired(); 
            modelBuilder.Entity<UserAddress>().Property(ua => ua.Street) // street
                .HasColumnType("nvarchar(max)").IsRequired(); 
            modelBuilder.Entity<UserAddress>().Property(ua => ua.BuildingNumber) // buildingNumber
                .HasColumnType("nvarchar(max)").IsRequired(); 
            modelBuilder.Entity<UserAddress>().Property(ua => ua.ApartmentNumber) // apartmentNumber
                .HasColumnType("nvarchar(max)");
            //====================================================================================================
            // [4] Category
            //=================
            // Table name
            modelBuilder.Entity<Category>().ToTable("Categories");
            // Primary key
            modelBuilder.Entity<Category>().HasKey(c => c.Id); // PK ID
            // Column Data Types
            modelBuilder.Entity<Category>().Property(c => c.Id) // ID
                .HasColumnType("int").UseIdentityColumn(1, 1); 
            modelBuilder.Entity<Category>().Property(c => c.Name) // Name
                .HasColumnType("nvarchar(max)").IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Description) // Description
                .HasColumnType("nvarchar(max)");
            modelBuilder.Entity<Category>().Property(c => c.Stock) // Stock ===> to be Updated Using a trigger 
                .HasColumnType("int").HasDefaultValue(0);
            //====================================================================================================
            // [5] Brand 
            //=================
            // Table name
            modelBuilder.Entity<Brand>().ToTable("Brands");
            // Primary key
            modelBuilder.Entity<Brand>().HasKey(b => b.Id); // PK ID
            // Column Data Types
            modelBuilder.Entity<Brand>().Property(b => b.Id) // ID
                .HasColumnType("int").UseIdentityColumn(1, 1);
            modelBuilder.Entity<Brand>().Property(b => b.Name) // Name
                .HasColumnType("nvarchar(max)").IsRequired();
            modelBuilder.Entity<Brand>().Property(b => b.Description) // Description
                .HasColumnType("nvarchar(max)");
            //====================================================================================================
            // [6] Product
            //=================
            // Table name
            modelBuilder.Entity<Product>().ToTable("Products");
            // Primary key
            modelBuilder.Entity<Product>().HasKey(p => p.Id); // PK ID
            // Column Data Types
            modelBuilder.Entity<Product>().Property(p => p.Id) // ID
                .HasColumnType("int").UseIdentityColumn(1000, 1);
            modelBuilder.Entity<Product>().Property(p => p.Name) // Name
                .HasColumnType("nvarchar(max)").IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Description) // Description
                .HasColumnType("nvarchar(max)");
            modelBuilder.Entity<Product>().Property(p => p.Price) // Price
                .HasColumnType("decimal(18,2)").IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.Image) // Image
                .HasColumnType("nvarchar(max)");
            modelBuilder.Entity<Product>().Property(p => p.Stock) // Stock
                .HasColumnType("int").IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.BrandId) // FK BrandId
                .HasColumnType("int").IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.CategoryId) // FK CategoryId
                .HasColumnType("int").IsRequired();
            modelBuilder.Entity<Product>().Property(p => p.DateAdded) // DateAdded
                .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            // Relationships
            // Brand (one) ===> Product (many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);
            // Category (one) ===> Product (many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            //====================================================================================================


            base.OnModelCreating(modelBuilder);
        }
    }
}
