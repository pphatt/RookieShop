namespace HeadphoneStore.StoreFrontEnd.Apis;

public class ProductApi
{
    /// <summary>
    /// Get lastest products.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Product/pagination?CategorySlug=tai-nghe&PageSize=10
    /// </summary>
    public const string GetProductByCategory = $"{BaseApi._devBaseUrl}/Product/pagination";
}