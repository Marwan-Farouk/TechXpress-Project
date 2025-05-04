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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // [7] Order
            //=================
            // Table name
            builder.ToTable("Orders");
            // Primary key
            builder.HasKey(o => o.Id); // PK ID
            // Column Data Types
            builder.Property(o => o.Id) // ID
                .HasColumnType("int").UseIdentityColumn(10, 1);
            builder.Property(o => o.UserId) // FK UserId
                .HasColumnType("int").IsRequired();
            builder.Property(o => o.OrderDate) // OrderDate
                .HasColumnType("datetime").HasDefaultValueSql("GETDATE()");
            builder.Property(o => o.TotalAmount) // TotalAmount
                .HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(o => o.Status) // Status
                .HasColumnType("nvarchar(max)").HasDefaultValue("pending");
            builder.Property(o => o.PaymentId) // FK PaymentId
                .HasColumnType("int");
            builder.Property(o => o.AddressId) // FK AddressId
                .HasColumnType("int").IsRequired();
            // Relationships
            // User (one) ===> Order (many)
            builder
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            // UserAddress (one) ===> Order (many)
            builder
                .HasOne(o => o.Address)
                .WithMany(ua => ua.Orders)
                .HasForeignKey(o => o.AddressId);
            // Payment (one) ===> Order (one)
            builder
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Order>(o => o.PaymentId);
            // OrderDetails (many) ===> Order (one)
            builder
                .HasMany(o => o.OrderItems)
                .WithOne(od => od.Order);
            //====================================================================================================
        }
    }
}
