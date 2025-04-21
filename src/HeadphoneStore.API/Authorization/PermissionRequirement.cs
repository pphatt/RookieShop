using Microsoft.AspNetCore.Authorization;

namespace HeadphoneStore.API.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; private set; }

    public PermissionRequirement(string Permission)
    {
        this.Permission = Permission;
    }
}
