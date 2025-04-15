using System.ComponentModel.DataAnnotations.Schema;

using HeadphoneStore.Domain.Abstracts.Entities;
using HeadphoneStore.Domain.Enumeration;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Domain.Aggregates.Identity.Entities;

[Table("AppUsers")]
public class AppUser : IdentityUser<Guid>, IAuditableEntity
{
    public string? FirstName { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTimeOffset? DayOfBirth { get; set; }
    public DateTimeOffset? LastLoginDate { get; set; }
    public string? Avatar { get; set; } = default!;
    public string? Bio { get; set; }
    public UserStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } // UserRoles
    public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; } // UserClaims
    public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; } // UserLogins
    public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; } // UserTokens

    private readonly List<UserAddress> _addresses = new();
    public virtual IReadOnlyCollection<UserAddress> Addresses => _addresses.AsReadOnly();
}
