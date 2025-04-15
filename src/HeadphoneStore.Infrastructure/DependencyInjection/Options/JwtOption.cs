namespace HeadphoneStore.Infrastructure.DependencyInjection.Options;

public class JwtOption
{
    public static string SelectionName { get; set; } = "JwtSettings";

    public string Secret { get; set; } = null!;

    public int ExpiryMinutes { get; init; }

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;
}
