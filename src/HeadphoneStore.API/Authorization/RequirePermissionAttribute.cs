using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HeadphoneStore.API.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly string[] _permissions;

    public RequirePermissionAttribute(params string[] permissions)
    {
        _permissions = permissions ?? Array.Empty<string>();

        if (_permissions.Length > 0)
        {
            Policy = string.Join(",", _permissions);
        }
    }

    public RequirePermissionAttribute(string function, string command)
    {
        string permissionValue = $"Permissions: {function} - {command}";
        _permissions = new[] { permissionValue };
        Policy = permissionValue;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
    }
}
