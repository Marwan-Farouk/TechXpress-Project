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

            builder.Property(u => u.FirstName) // FirstName
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(u => u.LastName) // LastName
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(u => u.BirthDate) // BirthDate
                .HasColumnType("datetime");
            builder.Property(u => u.DateCreated) // DateCreated
                .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");

            // Relationships

            // UserPhone (many) ===> User (one)

            // UserAddress (many) ===> User (one)
            builder
                .HasMany(u => u.userAddresses)
                .WithOne(ua => ua.User);
            //====================================================================================================
        }
    }
}
