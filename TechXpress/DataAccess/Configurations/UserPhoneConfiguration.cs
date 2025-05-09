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
    public class UserPhoneConfiguration : IEntityTypeConfiguration<UserPhone>
    {
        public void Configure(EntityTypeBuilder<UserPhone> builder)
        {
            // [2] UserPhone
            //=================
            // Table name
            builder.ToTable("UserPhones");
            // Primary key
            builder.HasKey(up => new { up.UserId, up.PhoneNumber });
            // Column Data Types
            builder.Property(up => up.UserId).HasColumnType("int"); // FK userId
            builder.Property(up => up.PhoneNumber).HasColumnType("nvarchar(11)"); // phoneNumber
            //====================================================================================================        
        }
    }
}
