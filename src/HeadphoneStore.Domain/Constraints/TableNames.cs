namespace HeadphoneStore.Domain.Constraints;

public class TableNames
{
    // *********** Plural Nouns ***********
    public const string AppUsers = nameof(AppUsers);
    public const string AppRoles = nameof(AppRoles);
    public const string AppUserRoles = nameof(AppUserRoles); // IdentityUserRole

    public const string AppUserClaims = nameof(AppUserClaims); // IdentityUserClaim
    public const string AppRoleClaims = nameof(AppRoleClaims); // IdentityRoleClaim
    public const string AppUserLogins = nameof(AppUserLogins); // IdentityUserLogin
    public const string AppUserTokens = nameof(AppUserTokens); // IdentityUserToken
}
