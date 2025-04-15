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

    public const string Permissions = nameof(Permissions);

    public const string Products = nameof(Products);
    public const string ProductMedias = nameof(ProductMedias);
    public const string Categories = nameof(Categories);
    public const string Orders = nameof(Orders);
    public const string OrderDetails = nameof(OrderDetails);
    public const string OrderPayments = nameof(OrderPayments);
    public const string Transactions = nameof(Transactions);
}
