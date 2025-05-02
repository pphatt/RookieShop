namespace HeadphoneStore.StoreFrontEnd.Apis;

public class ProductApi
{
    /// <summary>
    /// Get lastest products.
    /// </summary>
    public const string GetProductByCategory = $"{BaseApi._devBaseUrl}/Product/pagination";
}