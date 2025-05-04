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
    public class UserAddressConfiguration : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            // [3] UserAddress
            //=================
            // Table name
            builder.ToTable("UserAddresses");
            // Primary key
            builder.HasKey(ua =>  ua.AddressId );
            // Column Data Types
            builder.Property(ua => ua.UserId) // FK userId
                .HasColumnType("int");
            builder.Property(ua => ua.AddressId) // addressId
                .HasColumnType("int").UseIdentityColumn(1, 1);
            builder.Property(ua => ua.Country) // country
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(ua => ua.City) // city
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(ua => ua.Street) // street
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(ua => ua.BuildingNumber) // buildingNumber
                .HasColumnType("nvarchar(max)").IsRequired();
            builder.Property(ua => ua.ApartmentNumber) // apartmentNumber
                .HasColumnType("nvarchar(max)");
            //====================================================================================================
        }
    }
}
