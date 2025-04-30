using System.Security.Claims;

using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Services.Identity.Login;

using Microsoft.Extensions.Logging;

namespace HeadphoneStore.Application.UseCases.V1.Identity.Logout;

using Exceptions = Domain.Exceptions.Exceptions;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ICacheService _cacheService;
    private readonly ILogger<LogoutCommandHandler> _logger;

    public LogoutCommandHandler(IUserRepository userRepository,
                                ITokenService tokenService,
                                ICacheService cacheService,
                                ILogger<LogoutCommandHandler> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken); // it is not expired

        // principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
        // principal.Claims.FirstOrDefault(x => x.Type == UserClaims.Permissions);
        var emailKey = (principal.FindFirstValue(ClaimTypes.Email)?.ToString());

        if (emailKey is null)
            throw new Exceptions.Token.EmailKeyNotFound();

        var authenticated = await _cacheService.GetAsync<LoginResponseDto>($"User:{emailKey!}:Token:AuthenticatedToken", cancellationToken);

        if (authenticated is null)
            throw new Exceptions.Token.NotFoundInCached();

        await _cacheService.RemoveAsync($"User:{emailKey!}:Token:AuthenticatedToken", cancellationToken);

        var isUpdateSuccess = await _userRepository.UpdateLastLogin(emailKey);

        if (isUpdateSuccess == false)
        {
            _logger.LogError("Update last login time failed.");
        }

        return Result.Success();
    }
}
