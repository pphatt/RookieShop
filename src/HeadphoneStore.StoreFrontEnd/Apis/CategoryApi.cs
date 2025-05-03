namespace HeadphoneStore.StoreFrontEnd.Apis;

public class CategoryApi
{
    /// <summary>
    /// Get all categories with sub-categories.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Category/all
    /// </summary>
    public const string GetAllCategories = $"{BaseApi._devBaseUrl}/Category/all";
}
