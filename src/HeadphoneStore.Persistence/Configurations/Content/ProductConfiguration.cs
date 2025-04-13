using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Entities.Content;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CategoryId).IsRequired();
        builder.Property(p => p.Name).HasMaxLength(256).IsRequired();
        builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();
        builder.Property(p => p.Price).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.Sku).HasMaxLength(50).IsRequired();
        builder.Property(p => p.CreatedBy).IsRequired();
        builder.Property(p => p.CreatedOnUtc).IsRequired();
        builder.Property(p => p.IsDeleted).IsRequired();

        // One Product can have Many ProductMedia
        builder
            .HasMany(p => p.Media)
            .WithOne()
            .HasForeignKey("ProductId")
            .IsRequired();
    }
}
