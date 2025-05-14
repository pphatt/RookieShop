namespace HeadphoneStore.StoreFrontEnd.Apis.Endpoints;

public class CategoryApi
{
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Description: Get all categories with sub-categories.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Category/all
    /// </summary>
    public const string GetAllCategories = "Category/all-first-level";
    
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Description: Get all categories with sub-categories.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Category/all-with-sub
    /// </summary>
    public const string GetAllCategoriesWithSub = "Category/all-with-sub";
}
