using HeadphoneStore.Application.Abstracts.Interface.Services.Identity;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.API.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IIdentityService _identityService;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public PermissionAuthorizationHandler(IIdentityService identityService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _identityService = identityService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (_identityService.IsAuthenticated() == false)
        {
            context.Fail();
            return;
        }

        var user = await _userManager.FindByNameAsync(context.User.Identity!.Name!);

        if (user is null)
        {
            context.Fail();
            return;
        }

        // get all the user roles.
        var roleNames = await _userManager.GetRolesAsync(user);

        if (!roleNames.Any())
        {
            context.Fail();
            return;
        }

        var permissions = new List<Permission>();

        // Get all roles at once with their permissions
        var roles = await _roleManager.Roles
            .Where(r => roleNames.Contains(r.Name))
            .Include(r => r.Permissions)
            .ToListAsync();

        // Check if any role names weren't found
        var missingRoles = roleNames.Except(roles.Select(r => r.Name));

        if (missingRoles.Any())
        {
            context.Fail();
            return;
        }

        // Collect all permissions
        permissions = roles
            .SelectMany(r => r.Permissions)
            .ToList();

        // Filter permissions based on your criteria
        // Note: I'm assuming you want to check if the required permission exists in the list
        // You might need to adjust this part based on your actual Permission structure
        var hasRequiredPermission = permissions.Any(p =>
            p.Function == requirement.Function &&
            p.Command == requirement.Command);

        if (!hasRequiredPermission)
        {
            context.Fail();
            return;
        }

        context.Succeed(requirement);
    }
}
