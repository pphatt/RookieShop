using HeadphoneStore.Domain.Aggregates.Categories.Entities;
using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TableNames.Categories);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).HasMaxLength(256).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(1000).IsRequired();
        builder.Property(c => c.CreatedBy).IsRequired();
        builder.Property(c => c.CreatedOnUtc).IsRequired();
        builder.Property(c => c.IsDeleted).IsRequired();

        // Self-referencing relationship
        builder
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .IsRequired(false);

        // One Category can have Many Products
        builder
            .HasMany<Product>()
            .WithOne(p => p.Category)
            .HasForeignKey("CategoryId")
            .IsRequired();
    }
}
