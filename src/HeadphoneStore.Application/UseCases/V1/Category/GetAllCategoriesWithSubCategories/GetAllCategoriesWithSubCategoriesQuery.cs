using System.Text;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesWithSubCategories;

public sealed record GetAllCategoriesWithSubCategoriesQuery(string CategorySlug) : IQuery<List<CategoryDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append($"Categories:{nameof(GetAllCategoriesWithSubCategoriesQuery)}");

            if (CategorySlug != null)
            {
                builder.Append($":CategorySlug:{CategorySlug}");
            }

            builder.Append($":GetAll");

            return builder.ToString();
        }
    }
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
