using HeadphoneStore.Domain.Aggregates.Identity.Entities;

namespace HeadphoneStore.Application.Abstracts.Interface;

public interface IPermissionService
{
    Task<List<Permission>> GetPermissionsByRoles(string[] roleNames);

    Task<List<Permission>> GetPermissionsByRoles(AppUser user);
}
