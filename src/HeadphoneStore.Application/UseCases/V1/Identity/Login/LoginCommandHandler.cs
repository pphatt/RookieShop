using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Identity.Login;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Login;

using Exceptions = Domain.Exceptions.Exceptions;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResponseDto>
{
    private readonly ILogger<LoginCommandHandler> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _jwtService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IClaimsTransformation _claimsTransformation;
    private readonly ICacheService _cacheService;

    public LoginCommandHandler(
        ILogger<LoginCommandHandler> logger,
        SignInManager<AppUser> signInManager,
        ITokenService jwtTokenService,
        UserManager<AppUser> userManager,
        IClaimsTransformation claimsTransformation,
        ICacheService cacheService)
    {
        _logger = logger;
        _signInManager = signInManager;
        _jwtService = jwtTokenService;
        _userManager = userManager;
        _claimsTransformation = claimsTransformation;
        _cacheService = cacheService;
    }

    public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            throw new Exceptions.Identity.NotFound();

        //user.ValidateCommonRules();

        if (user.IsActive == false)
            throw new Exceptions.Identity.InactiveOrLockedOut();

        var loginStatus = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);

        if (loginStatus.Succeeded == false)
            throw new Exceptions.Identity.InvalidCredentials();

        var claims = await _claimsTransformation.TransformClaims(user);

        var response = new LoginResponseDto
        {
            AccessToken = _jwtService.GenerateAccessToken(claims),
            RefreshToken = _jwtService.GenerateRefreshToken(),
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
        };

        await _cacheService.SetAsync($"User:{user.Email!}:Token:AuthenticatedToken", response, null, cancellationToken);

        return Result.Success(response);
    }
}
