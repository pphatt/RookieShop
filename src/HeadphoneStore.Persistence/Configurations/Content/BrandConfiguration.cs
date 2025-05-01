using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.ToTable(TableNames.Brands);

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Name).HasMaxLength(256).IsRequired();
        builder.Property(b => b.Description).HasMaxLength(1000).IsRequired(false);
        builder.Property(p => p.Slug).HasMaxLength(256).IsRequired();
        builder.Property(b => b.CreatedBy).IsRequired();
        builder.Property(b => b.CreatedDateTime).IsRequired();
        builder.Property(b => b.UpdatedBy).IsRequired(false);
        builder.Property(b => b.UpdatedDateTime).IsRequired(false);
        builder.Property(b => b.IsDeleted).IsRequired();

        // Indexing slug
        builder
            .HasIndex(p => p.Slug)
            .IsUnique();

        // One Brand has many Products
        builder.HasMany(b => b.Products)
            .WithOne(p => p.Brand)
            .HasForeignKey(p => p.BrandId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(b => b.Name).IsUnique();
    }
}
