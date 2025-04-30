using System.ComponentModel;
using System.Reflection;

using HeadphoneStore.Domain.Constants;
using HeadphoneStore.Shared.Dtos.Identity.Role;

namespace HeadphoneStore.Application.DependencyInjection.Extensions;

public static class RoleClaimExtension
{
    public static void GetPermissionByType(this List<RoleClaimsDto> allPermissions, Type policy)
    {
        // get all the static const out of the type ("Roles", "User")
        FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

        foreach (FieldInfo field in fields)
        {
            string value = field.GetValue(null)!.ToString()!;

            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), true);

            var displayName = string.Empty;

            if (attributes.Any())
            {
                var description = (DescriptionAttribute)attributes[0];
                displayName = description.Description;
            }

            allPermissions.Add(new RoleClaimsDto
            {
                Value = value,
                Type = UserClaims.Permissions,
                DisplayName = displayName
            });
        }
    }
}
