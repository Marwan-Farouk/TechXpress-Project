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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // [4] Category
            //=================
            // Table name
            builder.ToTable("Categories");
            // Primary key
            builder.HasKey(c => c.Id); // PK ID
            // Column Data Types
            builder.Property(c => c.Id) // ID
                .HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(c => c.Name) // Name
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(c => c.Description) // Description
                .HasColumnType("nvarchar(max)");
            builder.Property(c => c.Stock) // Stock ===> to be Updated Using a trigger 
                .HasColumnType("int").HasDefaultValue(0);
            //====================================================================================================
        }
    }
}
