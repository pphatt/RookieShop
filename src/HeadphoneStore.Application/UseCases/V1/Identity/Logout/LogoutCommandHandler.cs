using System.Security.Claims;

using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Caching;
using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Services.Identity.Login;
using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Constants;

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

        var emailKey = (principal.FindFirstValue(ClaimTypes.Email)?.ToString());

        if (emailKey is null)
            throw new Exceptions.Token.EmailKeyNotFound();

        var cachedRefreshToken = await _cacheService.GetAsync<LoginResponseDto>($"User:{emailKey!}:Token:RefreshToken", cancellationToken);

        if (cachedRefreshToken is null)
            throw new Exceptions.Token.NotFoundInCached();

        await _cacheService.RemoveAsync($"User:{emailKey!}:Token:RefreshToken", cancellationToken);

        var isUpdateSuccess = await _userRepository.UpdateLastLogin(emailKey);

        if (isUpdateSuccess == false)
        {
            _logger.LogError("Update last login time failed.");
        }

        return Result.Success();
    }
}
