using HeadphoneStore.Domain.Aggregates.Products.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class ProductRatingConfiguration : IEntityTypeConfiguration<ProductRating>
{
    public void Configure(EntityTypeBuilder<ProductRating> builder)
    {
        builder.ToTable(TableNames.ProductRatings);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.RatingValue).IsRequired();
        builder.Property(r => r.Comment).HasMaxLength(1000).IsRequired(false);
        builder.Property(r => r.ProductId).IsRequired();
        builder.Property(r => r.CustomerId).IsRequired();

        builder
            .HasOne(r => r.Product)
            .WithMany(p => p.Ratings) 
            .HasForeignKey(r => r.ProductId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.Customer)
            .WithMany(u => u.ProductRatings)
            .HasForeignKey(r => r.CustomerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
