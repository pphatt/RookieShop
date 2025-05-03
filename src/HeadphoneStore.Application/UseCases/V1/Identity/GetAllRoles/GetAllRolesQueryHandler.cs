using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.Role;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllRoles;

public class GetAllRolesQueryHandler : IQueryHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly RoleManager<AppRole> _roleManager;

    public GetAllRolesQueryHandler(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<Result<List<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var query = await _roleManager.Roles
            .Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name,
                DisplayName = x.DisplayName,
                RoleStatus = x.Status.ToString()
            })
            .ToListAsync();

        return Result.Success(query);
    }
}
