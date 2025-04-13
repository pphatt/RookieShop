using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Entities.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HeadphoneStore.Persistence.Configurations.Identity;

internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable(TableNames.AppUsers);

        builder.HasKey(u => u.Id);

        builder.Property(x => x.FirstName).HasMaxLength(256);
        builder.Property(x => x.LastName).HasMaxLength(256);
        builder.Property(t => t.Email).HasMaxLength(256).IsRequired(true);
        builder.Property(t => t.Bio).IsRequired(false);

        // One User can have Many UserRoles
        builder
            .HasMany(x => x.UserRoles)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        // One User can have Many UserClaims
        builder
            .HasMany(x => x.Claims)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        // One User can have Many UserLogins
        builder
            .HasMany(x => x.Logins)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        // One User can have Many UserTokens
        builder
            .HasMany(x => x.Tokens)
            .WithOne()
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        // One User can have Many UserAddresses
        builder
            .HasMany(x => x.Addresses)
            .WithOne()
            .HasForeignKey(x => x.Id)
            .IsRequired();
    }
}
