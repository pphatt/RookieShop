using System.Text;

using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesPaged;

public class GetAllCategoriesPagedQuery : PagedDto, IQuery<PagedResult<CategoryDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append($"Categories:{nameof(GetAllCategoriesPagedQuery)}");

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
