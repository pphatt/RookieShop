using System.Security.Claims;

using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Services.Identity.Login;
using HeadphoneStore.Contract.Services.Identity.RefreshToken;
using HeadphoneStore.Domain.Aggregates.Identity.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;

using Exceptions = Domain.Exceptions.Exceptions;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponseDto>
{
    private readonly ITokenService _tokenService;
    private readonly ICacheService _cacheService;
    private readonly IClaimsTransformation _claimsTransformation;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(ITokenService tokenService,
                                      ICacheService cacheService,
                                      IClaimsTransformation claimsTransformation,
                                      UserManager<AppUser> userManager,
                                      ILogger<RefreshTokenCommandHandler> logger)
    {
        _tokenService = tokenService;
        _cacheService = cacheService;
        _claimsTransformation = claimsTransformation;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<Result<RefreshTokenResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var accessToken = request.AccessToken;
        var refreshToken = request.RefreshToken;

        if (string.IsNullOrWhiteSpace(accessToken))
            throw new Exceptions.Token.InvalidAccessToken();

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!);

        var emailKey = principal.FindFirstValue(ClaimTypes.Email)?.ToString();

        if (string.IsNullOrWhiteSpace(emailKey))
            throw new Exceptions.Token.NotFoundInCached();

        var authenticated = await _cacheService.GetAsync<LoginResponseDto>($"User:{emailKey!}:Token:AuthenticatedToken", cancellationToken);

        // After checking the user must be login again.
        // No need to check expired time because when it due it will be removed from cache.
        if (authenticated is null || authenticated.RefreshToken != refreshToken || authenticated.RefreshTokenExpiryTime <= DateTime.UtcNow)
            // less than datetime.now then login again
            throw new Exceptions.Token.NotFoundInCached();

        var user = await _userManager.FindByEmailAsync(emailKey);

        if (user == null)
            throw new Exceptions.Identity.NotFound();

        // why claims from fetch user:
        // - somehow the principle.Claims from access token contains duplicate properties so not gonna use that.
        var claims = await _claimsTransformation.TransformClaims(user);

        var newAccessToken = _tokenService.GenerateAccessToken(claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var newAuthenticated = new RefreshTokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1),
        };

        await _cacheService.SetAsync($"User:{emailKey!}:Token:AuthenticatedToken", newAuthenticated, null, cancellationToken);

        return Result.Success(newAuthenticated);
    }
}
