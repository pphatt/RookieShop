using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.Role;
using HeadphoneStore.Shared.Dtos.Identity.User;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetUserById;

using Exceptions = Domain.Exceptions.Exceptions;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<AppUser> _userManager;

    public GetUserByIdQueryHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        if (user.Status == EntityStatus.Inactive)
            throw new Exceptions.Identity.InactiveOrLockedOut();

        var result = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DayOfBirth = user.DayOfBirth,
            Avatar = user.Avatar,
            Bio = user.Bio,
            Status = user.IsActive.ToString(),
        };

        var roles = await _userManager.GetRolesAsync(user);

        if (roles is not null)
        {
            result.Roles = roles.Select(x => new RoleDto
            {
                DisplayName = x,
            }).First();
        }

        return Result.Success(result);
    }
}
