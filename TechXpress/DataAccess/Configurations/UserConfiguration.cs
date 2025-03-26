using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // [1] User
            //=================
            // Table name
            builder.ToTable("Users");
            // Primary key
            builder.HasKey(u => u.Id);
            // Column Data Types
            builder.Property(u => u.Id) // ID
                .HasColumnType("int").UseIdentityColumn(100, 1);
            builder.Property(u => u.FirstName) // FirstName
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(u => u.LastName) // LastName
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(u => u.Email) // Email
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(u => u.Password) // Password
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(u => u.BirthDate) // BirthDate
                .HasColumnType("datetime");
            builder.Property(u => u.DateCreated) // DateCreated
                .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            builder.Property(u => u.Role) // Role
                .HasColumnType("nvarchar(max)").HasDefaultValue("customer");
            // Relationships

            // UserPhone (many) ===> User (one)
            builder
                .HasMany(u => u.userPhones)
                .WithOne(up => up.User);

            // UserAddress (many) ===> User (one)
            builder
                .HasMany(u => u.userAddresses)
                .WithOne(ua => ua.User);
            //====================================================================================================
        }
    }
}
