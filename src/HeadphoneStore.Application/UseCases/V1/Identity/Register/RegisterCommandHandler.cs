using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Register;

using Exceptions = Domain.Exceptions.Exceptions;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public RegisterCommandHandler(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher)
    {
        _userManager = userManager;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email;
        var password = request.Password;

        var userFromDb = await _userManager.FindByEmailAsync(email);

        if (userFromDb is not null)
            throw new Exceptions.User.DuplicateEmail();

        var newUser = AppUser.Create(email);

        var hashedPassword = _passwordHasher.HashPassword(newUser, password);

        newUser.PasswordHash = hashedPassword;

        var createUserStatus = await _userManager.CreateAsync(newUser);

        if (createUserStatus.Succeeded == false)
        {
            var error = createUserStatus.Errors.First();
            throw new CustomException(error.Code, error.Description, ExceptionType.Failure);
        }

        var addRoleToPrivateUserStatus = await _userManager.AddToRoleAsync(newUser, "customer");

        if (addRoleToPrivateUserStatus.Succeeded == false)
        {
            var error = addRoleToPrivateUserStatus.Errors.First();
            throw new CustomException(error.Code, error.Description, ExceptionType.Failure);
        }

        return Result.Success();
    }
}
