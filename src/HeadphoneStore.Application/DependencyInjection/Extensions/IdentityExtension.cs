using System.Security.Claims;

using HeadphoneStore.Domain.Constants;

namespace HeadphoneStore.Application.DependencyInjection.Extensions;
public static class IdentityExtension
{
    public static string GetSpecificClaim(this IEnumerable<Claim> claimsIdentity, string claimType)
    {
        var claim = claimsIdentity.FirstOrDefault(x => x.Type == claimType);

        return (claim != null) ? claim.Value : string.Empty;
    }

    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        // can you ClaimsIdentity approach.
        // var userId = ((ClaimsIdentity)claimsPrincipal.Identity!).GetSpecificClaim(UserClaims.Id);
        var userId = claimsPrincipal.Claims.GetSpecificClaim(UserClaims.Id);

        return Guid.Parse(userId);
    }
}
