using HeadphoneStore.Domain.Aggregates.Order.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Content;

internal class OrderPaymentConfiguration : IEntityTypeConfiguration<OrderPayment>
{
    public void Configure(EntityTypeBuilder<OrderPayment> builder)
    {
        builder.ToTable(TableNames.OrderPayments);

        builder.HasKey(op => op.Id);

        builder.Property(op => op.CardNumber).HasMaxLength(20).IsRequired();
        builder.Property(op => op.Cvc).HasMaxLength(4).IsRequired();
        builder.Property(op => op.Expire).HasMaxLength(5).IsRequired();
        builder.Property(op => op.CreatedDateTime ).IsRequired();
        builder.Property(op => op.IsDeleted).IsRequired();
    }
}
