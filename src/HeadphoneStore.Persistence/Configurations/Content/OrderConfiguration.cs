using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Aggregates.Order.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(TableNames.Orders);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserId).IsRequired();
        builder.Property(o => o.Note).HasMaxLength(1000);
        builder.Property(o => o.Status).HasMaxLength(50).IsRequired();
        builder.Property(o => o.IsFeedback).IsRequired();
        builder.Property(o => o.TotalPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(o => o.CreatedDateTime).IsRequired();
        builder.Property(o => o.IsDeleted).IsRequired();

        // Order owns one OrderAddress
        builder.OwnsOne(o => o.ShippingAddress, a =>
        {
            a.Property(p => p.Street).HasMaxLength(500).IsRequired();
            a.Property(p => p.Province).HasMaxLength(100).IsRequired();
            a.Property(p => p.PhoneNumber).HasMaxLength(20).IsRequired();
        });

        // One AppUser can have Many Orders
        builder
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .IsRequired();

        // One Order can have Many OrderDetails
        builder
            .HasMany(o => o.OrderDetails)
            .WithOne(od => od.Order)
            .HasForeignKey(x => x.OrderId)
            .IsRequired();
    }
}
