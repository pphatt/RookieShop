using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Identity.UpdateUser;

using Exceptions = Domain.Exceptions.Exceptions;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    private readonly UserManager<AppUser> _userManager;

    public UpdateUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.Id.ToString());

        if (user is null)
            throw new Exceptions.Identity.NotFound();

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

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            throw new CustomException(
                code: result.Errors.ElementAt(0).Code,
                description: result.Errors.ElementAt(0).Description,
                type: ExceptionType.Unexpected
            );
        }

        return Result.Success("Update user successfully.");
    }
}
