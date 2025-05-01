using HeadphoneStore.Domain.Aggregates.Products.Entities;
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
        builder.Property(p => p.Slug).HasMaxLength(256).IsRequired();
        builder.Property(p => p.Stock).IsRequired();
        builder.Property(p => p.Sku).HasMaxLength(50).IsRequired();
        builder.Property(p => p.Sold).IsRequired();
        builder.Property(p => p.Status).IsRequired();
        builder.Property(p => p.ProductStatus).IsRequired();
        builder.Property(p => p.AverageRating).IsRequired();
        builder.Property(p => p.TotalReviews).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.CreatedDateTime).IsRequired();
        builder.Property(p => p.IsDeleted).IsRequired();

        // Indexing slug
        builder
            .HasIndex(p => p.Slug)
            .IsUnique();

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
            .WithOne(pm => pm.Product)
            .HasForeignKey(pm => pm.ProductId)
            .IsRequired(false);

        // One Product belongs to one Category
        builder
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();

        // One Product belongs to one Brand
        builder.HasOne(p => p.Brand)
            .WithMany(b => b.Products)
            .HasForeignKey(p => p.BrandId)
            .IsRequired(true);
    }
}
