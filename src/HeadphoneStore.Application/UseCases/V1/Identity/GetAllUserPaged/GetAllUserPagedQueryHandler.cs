using AutoMapper;

using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.Role;
using HeadphoneStore.Shared.Dtos.Identity.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllUserPaged;

public class GetAllUserPagedQueryHandler : IQueryHandler<GetAllUserPagedQuery, PagedResult<UserDto>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public GetAllUserPagedQueryHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<PagedResult<UserDto>>> Handle(GetAllUserPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _userManager.Users;
        var roles = _roleManager.Roles.AsQueryable();

        query = query.Where(u => !u.UserRoles.Any(ur => roles.FirstOrDefault(x => x.Name == "admin").Id == ur.RoleId));

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            query = query.Where(
                user => user.Email!.Contains(request.SearchTerm) ||
                        user.UserName!.Contains(request.SearchTerm) ||
                        user.FirstName!.Contains(request.SearchTerm) ||
                        user.LastName!.Contains(request.SearchTerm) ||
                        user.PhoneNumber!.Contains(request.SearchTerm)
            );
        }

        var count = await query.CountAsync();

        var pageIndex = request.PageIndex < 0 ? 1 : request.PageIndex;
        var skipPages = (pageIndex - 1) * request.PageSize;

        query = query
            .Skip(skipPages)
            .Take(request.PageSize);

        var users = await query.Select(x => new UserDto
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            PhoneNumber = x.PhoneNumber,
            DayOfBirth = x.DayOfBirth,
            Avatar = x.Avatar,
            Roles = x.UserRoles.Select(r => new RoleDto
            {
                Id = roles.FirstOrDefault(x => x.Id == r.RoleId)!.Id,
                DisplayName = roles.FirstOrDefault(x => x.Id == r.RoleId)!.DisplayName ?? "Customer",
            }).First(),
            Status = x.Status.ToString(),
        }).ToListAsync();

        var result = new PagedResult<UserDto>(
            items: users,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            totalCount: count);

        return Result.Success(result);
    }
}
