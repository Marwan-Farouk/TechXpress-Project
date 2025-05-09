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
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            // [8] Payment
            //=================
            // Table name
            builder.ToTable("Payments");
            // Primary key
            builder.HasKey(p => p.Id); // PK ID
            // Column Data Types
            builder.Property(p => p.Id) // ID
                .HasColumnType("int").UseIdentityColumn(10, 1);
            builder.Property(p => p.Amount) // Amount
                .HasColumnType("decimal(18,2)").IsRequired();
            builder.Property(p => p.OrderId) // ID
                .HasColumnType("int").IsRequired();
            builder.Property(p => p.ProviderId) // ProviderId
                .HasColumnType("int").IsRequired();
            builder.Property(p => p.Status) // Status
                .HasColumnType("nvarchar(max)").IsRequired();
            // Relationships
            // Order (one) ===> Payment (one)   
            builder
                .HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId);
            // Provider (one) ===> Payment (many)
            builder
                .HasOne(p => p.Provider)
                .WithMany(pr => pr.Payments)
                .HasForeignKey(p => p.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);
            //====================================================================================================
        }
    }
}
