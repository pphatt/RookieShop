using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Constraints;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Identity.DeleteUser;

using Exceptions = Domain.Exceptions.Exceptions;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ICacheService _cacheService;

    public DeleteUserCommandHandler(
        UserManager<AppUser> userManager,
        ICacheService cacheService)
    {
        _userManager = userManager;
        _cacheService = cacheService;
    }

    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Any())
            throw new Exceptions.Role.NotFound();

        if (roles.Contains(Roles.Admin))
            throw new Exceptions.Identity.CannotDelete();

        var roleRemovalResult = await _userManager.RemoveFromRolesAsync(user, roles);

        if (!roleRemovalResult.Succeeded)
            throw new CustomException(
                code: roleRemovalResult.Errors.ElementAt(0).Code,
                description: roleRemovalResult.Errors.ElementAt(0).Description,
                type: ExceptionType.Forbidden
            );

        await _cacheService.RemoveAsync($"User:{user.Email!}:Token:AuthenticatedToken", cancellationToken);

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
            throw new CustomException(
                code: result.Errors.ElementAt(0).Code,
                description: result.Errors.ElementAt(0).Description,
                type: ExceptionType.Unexpected
            );

        return Result.Success("Delete user successfully.");
    }
}
