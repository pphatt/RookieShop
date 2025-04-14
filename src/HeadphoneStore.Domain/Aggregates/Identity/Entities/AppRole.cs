using System.ComponentModel.DataAnnotations.Schema;

using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Domain.Aggregates.Identity.Entities;

[Table("AppRoles")]
public class AppRole : IdentityRole<Guid>
{
    public string DisplayName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public RoleStatus Status { get; set; }

    public virtual ICollection<IdentityUserRole<Guid>> UserRoles { get; set; } // UserRoles
    public virtual ICollection<IdentityRoleClaim<Guid>> Claims { get; set; } // RoleClaims
}
