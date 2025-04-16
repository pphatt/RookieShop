using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Enumerations;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();
        builder.Property(p => p.Quantity).IsRequired();
        builder.Property(p => p.Sku).HasMaxLength(50).IsRequired();
        builder.Property(p => p.ProductStatus).IsRequired();
        builder.Property(p => p.AverageRating).IsRequired();
        builder.Property(p => p.TotalReviews).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.CreatedDateTime ).IsRequired();
        builder.Property(p => p.IsDeleted).IsRequired();

        // Setup ProductPrice ValueObject
        builder.OwnsOne(c => c.ProductPrice, price =>
        {
            price.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();
        });

        // One Product can have Many ProductMedia
        builder
            .HasMany(p => p.Media)
            .WithOne()
            .HasForeignKey("ProductId")
            .IsRequired();

        // One Product belongs to one Category
        builder
            .HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();
    }
}
