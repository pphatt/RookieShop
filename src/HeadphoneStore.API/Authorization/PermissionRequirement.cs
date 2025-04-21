using Microsoft.AspNetCore.Authorization;

namespace HeadphoneStore.API.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string Permission { get; private set; }
    public string Function { get; private set; }
    public string Command { get; private set; }

    private const string POLICY_PREFIX = "Permissions";

    public PermissionRequirement(string permission)
    {
        Permission = permission;

        var parts = permission.Split(' ');

        if (parts.Length == 4)
        {
            Function = $"{POLICY_PREFIX}.Function.{parts[1]}";
            Command = $"{POLICY_PREFIX}.Command.{parts[3]}";
        }
        else
        {
            Function = permission;
            Command = string.Empty;
        }
    }
}
