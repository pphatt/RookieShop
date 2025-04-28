using AutoMapper;

using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Identity.User;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllUserPaged;

public class GetAllUserPagedQueryHandler : IQueryHandler<GetAllUserPagedQuery, PagedResult<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;

    public GetAllUserPagedQueryHandler(IMapper mapper, UserManager<AppUser> userManager)
    {
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<Result<PagedResult<UserDto>>> Handle(GetAllUserPagedQuery request, CancellationToken cancellationToken)
    {
        var query = _userManager.Users;

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
            Bio = x.Bio,
            UserStatus = x.IsActive.ToString(),
        }).ToListAsync();

        var result = new PagedResult<UserDto>(
            items: users,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize,
            totalCount: count);

        return Result.Success(result);
    }
}
