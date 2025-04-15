using System.Security.Claims;

using HeadphoneStore.Domain.Aggregates.Identity.Entities;

namespace HeadphoneStore.Application.Abstracts.Interface;

public interface IClaimsTransformation
{
    Task<List<Claim>> TransformClaims(AppUser user);
}
