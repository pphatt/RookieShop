using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Infrastructure.Services.Authentication;

public class PermissionService : IPermissionService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public PermissionService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<Permission>> GetPermissionsByRoles(string[] roleNames)
    {
        var roles = await _roleManager.Roles
            .Where(r => roleNames.Contains(r.Name))
            .Include(r => r.Permissions)
            .Select(r => new { r.Permissions, r.Name })
            .ToListAsync();

        var missingRoles = roleNames.Except(roles.Select(r => r.Name));

        if (missingRoles.Any())
        {
            throw new Exception($"the following roles were not found: {string.Join(", ", missingRoles)}");
        }

        var permissions = roles
            .SelectMany(r => r.Permissions)
            .Distinct()
            .ToList();

        return [.. permissions];
    }

    public async Task<List<Permission>> GetPermissionsByRoles(AppUser user)
    {
        var roleNames = await _userManager.GetRolesAsync(user);
        return await GetPermissionsByRoles([.. roleNames]);
    }
}
