using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumeration;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Identity.Role;
using HeadphoneStore.Shared.Dtos.Identity.User;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Identity.WhoAmI;

using Exceptions = Domain.Exceptions.Exceptions;

public class WhoAmIQueryHandler : IQueryHandler<WhoAmIQuery, UserDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly ICacheService _cacheService;

    public WhoAmIQueryHandler(UserManager<AppUser> userManager,
                              RoleManager<AppRole> roleManager,
                              ICacheService cacheService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _cacheService = cacheService;
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
            throw new Exceptions.Identity.NotFound();

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
            Status = Enum.GetName(typeof(UserStatus), (int)userFromDb.Status) ?? UserStatus.Inactive.ToString(),
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
            }).First()
        };

        await _cacheService.SetAsync($"User:{userFromDb.Email!}:Profile:WhoAmI", response, null, cancellationToken);

        return Result.Success(response);
    }
}
