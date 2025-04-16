using System.Security.Claims;

namespace HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
