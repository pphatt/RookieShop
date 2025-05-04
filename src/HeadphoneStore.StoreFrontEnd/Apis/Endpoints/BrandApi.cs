namespace HeadphoneStore.StoreFrontEnd.Apis.Endpoints;

public class BrandApi
{
    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Description: Get all brands.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Brand/all
    /// </summary>
    public const string GetAllBrands = "Brand/all";

    /// <summary>
    /// [AllowAnonymous]
    /// <br />
    /// Description: Get all brands filtered by product properties.
    /// <br />
    /// Sample endpoint: https://localhost:8081/api/v1/Brand/all-brands-filtered-by-product-properties
    /// </summary>
    public const string GetAllBrandsFilteredByProductProperties = "Brand/all-brands-filtered-by-product-properties";
}