namespace HeadphoneStore.StoreFrontEnd.Apis.Endpoints;

public class ProductApi
{
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Get product by id.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Product/:id
    /// </summary>
    public const string GetProduct = "Product";
    
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Get lastest products.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Product/pagination?CategorySlug=tai-nghe&PageSize=10
    /// </summary>
    public const string GetAllProducts = "Product/pagination";
}