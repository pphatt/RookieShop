using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Constants;
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
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(UserClaims.Id, user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.UserName ?? string.Empty),
            new(UserClaims.Username, user.UserName ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
            new(UserClaims.Email, user.Email ?? string.Empty),
            new(UserClaims.PhoneNumber, user.PhoneNumber ?? string.Empty),
            new(UserClaims.Avatar, user.Avatar ?? string.Empty),
            new(UserClaims.Status, user.Status.ToString() ?? UserStatus.Inactive.ToString()),
            new(UserClaims.Roles, JsonSerializer.Serialize(roleNames)),
            new(UserClaims.Permissions, JsonSerializer.Serialize(permissionClaims)),
        ];
    }
}
