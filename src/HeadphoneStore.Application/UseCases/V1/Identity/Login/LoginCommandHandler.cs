using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Domain.Exceptions;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponse>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _jwtService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IClaimsTransformation _claimsTransformation;

    public LoginCommandHandler(
        ILogger<LoginCommandHandler> logger,
        SignInManager<AppUser> signInManager,
        ITokenService jwtTokenService,
        UserManager<AppUser> userManager,
        IClaimsTransformation claimsTransformation)
    {
        _logger = logger;
        _signInManager = signInManager;
        _jwtService = jwtTokenService;
        _userManager = userManager;
        _claimsTransformation = claimsTransformation;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            throw new UserException.NotFound();
        }

        //user.ValidateCommonRules();

        if (user.IsActive == false)
        {
            throw new UserException.InactiveOrLockedOut();
        }

        var loginStatus = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (loginStatus.Succeeded == false)
        {
            throw new UserException.InvalidCredentials();
        }

        var claims = await _claimsTransformation.TransformClaims(user);

        var response = new LoginResponse
        {
            AccessToken = _jwtService.GenerateAccessToken(claims),
            RefreshToken = _jwtService.GenerateRefreshToken(),
        };

        return Result.Success(response);
    }
}
