using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // [6] Product
            //=================
            // Table name
            builder.ToTable("Products");
            // Primary key
            builder.HasKey(p => p.Id); // PK ID
            // Column Data Types
            builder.Property(p => p.Id) // ID
                .HasColumnType("int").UseIdentityColumn(1000, 1);
            builder.Property(p => p.Name) // Name
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(p => p.Description) // Description
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Price) // Price
                .HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.Image) // Image
                .HasColumnType("nvarchar(max)");
            builder.Property(p => p.Stock) // Stock
                .HasColumnType("int").IsRequired();
            builder.Property(p => p.BrandId) // FK BrandId
                .HasColumnType("int").IsRequired();
            builder.Property(p => p.CategoryId) // FK CategoryId
                .HasColumnType("int").IsRequired();
            builder.Property(p => p.DateAdded) // DateAdded
                .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            // Relationships
            // Brand (one) ===> Product (many)
            builder
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId);
            // Category (one) ===> Product (many)
            builder
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            //====================================================================================================
        }
    }
}
