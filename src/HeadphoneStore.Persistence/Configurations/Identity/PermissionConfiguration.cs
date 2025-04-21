using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Constraints;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Identity;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder
            .ToTable(TableNames.Permissions)
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Function)
            .HasMaxLength(256)
            .IsRequired(true);

        builder.Property(x => x.Command)
            .HasMaxLength(256)
            .IsRequired(true);

        // admin - product - view
        // admin - product - create
        builder
            .HasIndex(x => new { x.Id, x.Function, x.Command })
            .IsUnique();

        builder
            .HasOne(p => p.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(p => p.RoleId)
            .IsRequired();
    }
}
