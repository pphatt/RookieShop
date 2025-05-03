using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Domain.Exceptions;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public UpdateUserCommandHandler(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

        if (role is null)
            throw new Exceptions.Role.NotFound();

        if (!string.IsNullOrWhiteSpace(request.FirstName))
        {
            user.FirstName = request.FirstName;
        }

        if (!string.IsNullOrWhiteSpace(request.LastName))
        {
            user.LastName = request.LastName;
        }

        if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
        {
            user.PhoneNumber = request.PhoneNumber;
        }

        // remove current role.
        var currentRole = await _userManager.GetRolesAsync(user);
        var roleRemovalResult = await _userManager.RemoveFromRolesAsync(user, currentRole);

        if (!roleRemovalResult.Succeeded)
            throw new CustomException(
                code: roleRemovalResult.Errors.First().Code,
                description: roleRemovalResult.Errors.First().Description,
                type: ExceptionType.Validation
            );

        // add new role.
        var addedRoleResult = await _userManager.AddToRoleAsync(user, role.Name!);

        if (!addedRoleResult.Succeeded)
            throw new CustomException(
                code: addedRoleResult.Errors.First().Code,
                description: addedRoleResult.Errors.First().Description,
                type: ExceptionType.Validation
            );

        Enum.TryParse<EntityStatus>(request.Status, true, out var status);

        user.Status = status;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new CustomException(
                code: result.Errors.First().Code,
                description: result.Errors.First().Description,
                type: ExceptionType.Unexpected
            );
        }

        return Result.Success("Update user successfully.");
    }
}
