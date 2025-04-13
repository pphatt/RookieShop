using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Entities.Content;

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

        // One Category can have Many Products
        builder
            .HasMany<Product>()
            .WithOne()
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();
    }
}
