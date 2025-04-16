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
        builder.Property(od => od.ProductQuantity).IsRequired();
        builder.Property(od => od.ProductPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(od => od.ProductPriceDiscount).HasPrecision(18, 2);
        builder.Property(od => od.CreatedBy).IsRequired();
        builder.Property(od => od.CreatedDateTime ).IsRequired();
        builder.Property(od => od.IsDeleted).IsRequired();

        // One OrderDetail belongs to One Order
        builder
            .HasOne<Order>()
            .WithMany(o => o.OrderDetails)
            .HasForeignKey("OrderId")
            .IsRequired();

        // One Product can have Many OrderDetails
        builder
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(o => o.ProductId)
            .IsRequired();
    }
}
