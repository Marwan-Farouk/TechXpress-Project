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
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            // [5] Brand 
            //=================
            // Table name
            builder.ToTable("Brands");
            // Primary key
            builder.HasKey(b => b.Id); // PK ID
            // Column Data Types
            builder.Property(b => b.Id) // ID
                .HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(b => b.Name) // Name
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(b => b.Description) // Description
                .HasColumnType("nvarchar(max)");
            //====================================================================================================
        }
    }
}
