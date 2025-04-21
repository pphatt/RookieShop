using HeadphoneStore.Application.Abstracts.Interface.Services.Identity;
using HeadphoneStore.Application.DependencyInjection.Extensions;

using Microsoft.AspNetCore.Http;

namespace HeadphoneStore.Infrastructure.Services.Identity;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
        => _httpContextAccessor
            .HttpContext!
            .User
            .GetUserId();

    public bool? IsAuthenticated()
        => _httpContextAccessor
            .HttpContext?
            .User
            .Identity?
            .IsAuthenticated;
}
