using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Identity;

internal class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable(TableNames.AppRoles);

        builder.HasKey(r => r.Id);

        builder.Property(t => t.Name).HasMaxLength(256).IsRequired(true);
        builder.Property(t => t.DisplayName).HasMaxLength(256).IsRequired(true);
        builder.Property(t => t.Description).HasMaxLength(256).IsRequired(true);

        // One Role can have Many RolesClaims
        builder
            .HasMany(x => x.Claims)
            .WithOne()
            .HasForeignKey(x => x.RoleId)
            .IsRequired();

        // One User can have Many UserRoles
        builder
            .HasMany(x => x.UserRoles)
            .WithOne()
            .HasForeignKey(x => x.RoleId)
            .IsRequired();
    }
}
