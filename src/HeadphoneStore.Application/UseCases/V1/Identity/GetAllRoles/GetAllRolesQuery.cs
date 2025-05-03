using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.Role;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllRoles;

public class GetAllRolesQuery : IQuery<List<RoleDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey => $"Role:{nameof(GetAllRolesQuery)}:GetAll";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
