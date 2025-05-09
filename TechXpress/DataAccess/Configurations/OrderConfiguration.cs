using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            // Configure StripeSessionId
            builder.Property(o => o.StripeSessionId)
                .HasMaxLength(255) // Set max length to 255
                .IsRequired(false); // It's nullable

            // Relationships
            builder
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(o => o.Address)
                .WithMany(ua => ua.Orders)
                .HasForeignKey(o => o.AddressId);

            builder
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Order>(o => o.PaymentId);

            builder
                .HasMany(o => o.OrderItems)
                .WithOne(od => od.Order);
        }
    }
}
