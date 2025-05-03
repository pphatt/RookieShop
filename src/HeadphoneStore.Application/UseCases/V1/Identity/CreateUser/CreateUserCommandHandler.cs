using HeadphoneStore.Application.Abstracts.Interface.Services.Mail;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Domain.Exceptions;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;

using Microsoft.AspNetCore.Identity;

namespace HeadphoneStore.Application.UseCases.V1.Identity.CreateUser;

using Exceptions = Domain.Exceptions.Exceptions;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IEmailService _emailService;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IEmailService emailService,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _emailService = emailService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var IsEmailExisted = await _userManager.FindByEmailAsync(request.Email);

        if (IsEmailExisted is not null)
            throw new Exceptions.Identity.DuplicateEmail();

        var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());

        if (role is null)
            throw new Exceptions.Role.NotFound();

        Enum.TryParse<EntityStatus>(request.Status, true, out var status);

        var newUser = AppUser.Create(
            email: request.Email,
            firstName: request.FirstName,
            lastName: request.LastName,
            phoneNumber: request.PhoneNumber
        );

        string password = GenerateRandomPassword(12);
        string hashPassword = new PasswordHasher<AppUser>().HashPassword(newUser, password);

        newUser.PasswordHash = hashPassword;
        newUser.Status = status;

        var createNewUserResult = await _userManager.CreateAsync(newUser);

        if (!createNewUserResult.Succeeded)
            throw new CustomException(
                code: createNewUserResult.Errors.ElementAt(0).Code,
                description: createNewUserResult.Errors.ElementAt(0).Description,
                type: ExceptionType.Validation);

        var saveUserRoleResult = await _userManager.AddToRoleAsync(newUser, role.Name!);

        if (!saveUserRoleResult.Succeeded)
            throw new CustomException(
                code: saveUserRoleResult.Errors.ElementAt(0).Code,
                description: saveUserRoleResult.Errors.ElementAt(0).Description,
                type: ExceptionType.Validation);

        // handle send email.
        await _emailService.SendEmailAsync(new EmailContent
        {
            ToEmail = request.Email,
            Subject = "Account information",
            Body = $@"Hi {newUser.UserName},<br><br>
                    Your account has been successfully created. Below are your login credentials:<br><br>
                    <strong>Email:</strong> {newUser.Email}<br>
                    <strong>Password:</strong> {password}<br><br>
                    For security reasons, we recommend changing your password upon first login.<br><br>
                    Best regards,<br>
                    The Account Security Team"
        });

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Create new user successfully.");
    }

    private static Random random = new Random();

    private static string GenerateRandomPassword(int length)
    {
        const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercase = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string specialChars = "!@#$%^&*()";
        const string allChars = uppercase + lowercase + digits + specialChars;

        return new string(Enumerable.Repeat(allChars, length).Select(c => c[random.Next(c.Length)]).ToArray());
    }
}
