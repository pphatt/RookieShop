using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Entities.Content;
using HeadphoneStore.Domain.Entities.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable(TableNames.Transactions);

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Total).HasPrecision(18, 2).IsRequired();
        builder.Property(t => t.Note).HasMaxLength(1000).IsRequired();
        builder.Property(t => t.UserId).IsRequired();
        builder.Property(t => t.Status).HasMaxLength(50).IsRequired();
        builder.Property(t => t.CreatedBy).IsRequired();
        builder.Property(t => t.CreatedOnUtc).IsRequired();
        builder.Property(t => t.IsDeleted).IsRequired();

        // One Transaction is associated with one User
        builder
            .HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .IsRequired();
    }
}
