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
    public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
    {
        public void Configure(EntityTypeBuilder<OrderDetails> builder)
        {
            // [9] OrderDetails
            //=================
            // Table name
            builder.ToTable("OrderDetails");
            // Primary key
            builder.HasKey(od => new { od.OrderId, od.ProductId }); // PK OrderId, ProductId
                                                                                                // Column Data Types
            builder.Property(od => od.OrderId) // FK OrderId
                .HasColumnType("int").IsRequired();
            builder.Property(od => od.ProductId) // FK ProductId
                .HasColumnType("int").IsRequired();
            builder.Property(od => od.Quantity) // Quantity
                .HasColumnType("int").IsRequired();
            builder.Property(od => od.UnitPrice) // UnitPrice
                .HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(od => od.Discount) // Discount
                .HasColumnType("decimal(18,2)").IsRequired();
            // Relationships
            // Order (one) ===> OrderDetails (many)
            builder
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(od => od.OrderId);
            // Product (one) ===> OrderDetails (many)
            builder
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId);
            //====================================================================================================
        }
    }
}
