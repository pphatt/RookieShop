using System.Text;

using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;

public class GetAllProductsPagedQuery : PagedDto, IQuery<PagedResult<ProductDto>>, ICacheable
{
    public string? CategorySlug { get; set; }

    public bool BypassCache => false;
    public string CacheKey
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append($"Products:{nameof(GetAllProductsPagedQuery)}");

            if (SearchTerm != null)
            {
                builder.Append($":SearchTerm:{SearchTerm}");
            }

            if (CategorySlug != null)
            {
                builder.Append($":Category:{CategorySlug}");
            }

            builder.Append($":Page:{PageIndex}:{PageSize}");

            return builder.ToString();
        }
    }
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
