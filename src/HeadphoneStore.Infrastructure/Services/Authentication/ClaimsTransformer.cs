using System.Security.Claims;
using System.Text.Json;

using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using HeadphoneStore.Domain.Enumeration;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Infrastructure.Services.Authentication;

public class ClaimsTransformer : IClaimsTransformation
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPermissionService _permissionService;

    public ClaimsTransformer(UserManager<AppUser> userManager, IPermissionService permissionService)
    {
        _userManager = userManager;
        _permissionService = permissionService;
    }

    private List<string> TransformPermissionsToPermissionClaims(List<Permission> permissions)
    {
        return permissions
            .Select(p => p.GetDisplayName())
            .ToList();
    }

    public async Task<List<Claim>> TransformClaims(AppUser user)
    {
        var roleNames = await _userManager.GetRolesAsync(user);
        var permissions = await _permissionService.GetPermissionsByRoles(user);
        var permissionClaims = TransformPermissionsToPermissionClaims(permissions);

        return [
            new("id", user.Id.ToString()),
            new("email", user.Email ?? string.Empty),
            //new("fullname", user.GetFullName() ?? string.Empty),
            new("phoneNumber", user.PhoneNumber ?? string.Empty),
            new("status", user.Status.ToString() ?? UserStatus.Inactive.ToString()),
            new("roles", JsonSerializer.Serialize(roleNames)),
            new("permissions", JsonSerializer.Serialize(permissionClaims)),
        ];
    }
}
