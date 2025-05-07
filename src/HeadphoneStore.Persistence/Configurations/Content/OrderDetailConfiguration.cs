using HeadphoneStore.Domain.Aggregates.Order.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable(TableNames.OrderDetails);

        builder.HasKey(od => od.Id);

        builder.Property(od => od.ProductId).IsRequired();
        builder.Property(od => od.Quantity).IsRequired();
        builder.Property(od => od.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(od => od.CreatedDateTime).IsRequired();
        builder.Property(od => od.IsDeleted).IsRequired();

        // One OrderDetail belongs to One Order
        builder
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderId)
            .IsRequired();

        // One Product can have Many OrderDetails
        builder
            .HasOne(od => od.Product)
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .IsRequired();
    }
}
