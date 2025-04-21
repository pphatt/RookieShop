using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using HeadphoneStore.Application.Abstracts.Interface.Services.Authentication;
using HeadphoneStore.Application.Abstracts.Interface.Services.Datetime;
using HeadphoneStore.Infrastructure.DependencyInjection.Options;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HeadphoneStore.Infrastructure.Services.Authentication;

public class JwtTokenService : ITokenService
{
    private const string _secretKey = "z6_Fpg5YXVIfY{vD+!AJm)oYHP.#;t~)&tyadtB80m8T7]Z'CGcU0VXO~Rl5_qOg_&(%NX$3c8G,0'.'MbzSkJ+AxcI7ViY@DNbN";

    private readonly JwtOption _jwtOption;

    public JwtTokenService(IOptions<JwtOption> jwtOptions)
    {
        _jwtOption = jwtOptions.Value;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = claims.ToDictionary(x => x.Type, x => x.Value as object),
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(_jwtOption.ExpiryMinutes),
            Issuer = _jwtOption.Issuer,
            SigningCredentials = signingCredentials
        };

        var tokenString = new JwtSecurityTokenHandler().CreateEncodedJwt(tokenDescriptor);

        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);

        return Convert.ToBase64String(bytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true, // you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = true,
            ValidateLifetime = true, // here we are saying that we do need to care about the token's expiration date
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}
