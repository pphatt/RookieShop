using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Identity.Role;
using HeadphoneStore.Contract.Dtos.Identity.User;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumeration;
using HeadphoneStore.Domain.Enumerations;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;

using Exceptions = Domain.Exceptions.Exceptions;

public class WhoAmIQueryHandler : IQueryHandler<WhoAmIQuery, UserDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public WhoAmIQueryHandler(UserManager<AppUser> userManager,
                              RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result<UserDto>> Handle(WhoAmIQuery request, CancellationToken cancellationToken)
    {
        var userFromDb = await _userManager
            .Users
            .Include(x => x.UserRoles)
            .Include(x => x.Addresses)
            .Where(x => x.Id.ToString() == request.UserId)
            .FirstOrDefaultAsync();

        if (userFromDb is null)
            throw new Exceptions.User.NotFound();

        var userRoles = userFromDb.UserRoles.Select(x => x.RoleId).ToList();
        var roles = await _roleManager.Roles.Where(x => userRoles.Contains(x.Id)).ToListAsync(cancellationToken);

        var response = new UserDto
        {
            Id = userFromDb.Id,
            FirstName = userFromDb.FirstName,
            LastName = userFromDb.LastName,
            Email = userFromDb.Email!,
            PhoneNumber = userFromDb.PhoneNumber!,
            DayOfBirth = userFromDb.DayOfBirth,
            Avatar = userFromDb.Avatar,
            Bio = userFromDb.Bio,
            UserStatus = Enum.GetName(typeof(UserStatus), (int)userFromDb.Status) ?? UserStatus.Inactive.ToString(),
            CreatedDateTime = userFromDb.CreatedDateTime,
            UpdatedDateTime = userFromDb.UpdatedDateTime,
            UserAddress = userFromDb.Addresses.Select(x => new UserAddressDto
            {
                Address = x.Address,
                Street = x.Street,
                Province = x.Province,
                PhoneNumber = x.PhoneNumber
            }),
            Roles = roles.Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name!,
                DisplayName = x.DisplayName,
                RoleStatus = Enum.GetName(typeof(RoleStatus), (int)x.Status) ?? RoleStatus.Inactive.ToString(),
            })
        };

        return Result.Success(response);
    }
}
