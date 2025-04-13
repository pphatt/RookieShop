using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Entities.Content;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class ProductMediaConfiguration : IEntityTypeConfiguration<ProductMedia>
{
    public void Configure(EntityTypeBuilder<ProductMedia> builder)
    {
        builder.ToTable(TableNames.ProductMedias);

        builder.HasKey(m => m.Id);

        builder.Property(m => m.ImageUrl).HasMaxLength(1000).IsRequired();
        builder.Property(m => m.CreatedBy).IsRequired();
        builder.Property(m => m.CreatedOnUtc).IsRequired();
        builder.Property(m => m.IsDeleted).IsRequired();
    }
}
