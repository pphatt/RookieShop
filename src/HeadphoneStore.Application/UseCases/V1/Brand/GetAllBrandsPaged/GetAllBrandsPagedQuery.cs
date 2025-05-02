using System.Text;

using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;

public class GetAllBrandsPagedQuery : PagedDto, IQuery<PagedResult<BrandDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append($"Brands:{nameof(GetAllBrandsPagedQuery)}");

            if (SearchTerm != null)
            {
                builder.Append($":SearchTerm:{SearchTerm}");
            }

            builder.Append($":Page:{PageIndex}:{PageSize}");

            return builder.ToString();
        }
    }
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
