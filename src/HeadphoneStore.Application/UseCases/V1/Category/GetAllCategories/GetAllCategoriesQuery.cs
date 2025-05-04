using System.Text;

using HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;

public class GetAllCategoriesQuery : IQuery<List<CategoryDto>>, ICacheable
{
    public string? SearchTerm { get; set; }

    public bool BypassCache => false;
    public string CacheKey
    {
        get
        {
            var builder = new StringBuilder();
            builder.Append($"Categories:{nameof(GetAllCategoriesQuery)}");

            if (SearchTerm != null)
            {
                builder.Append($":SearchTerm:{SearchTerm}");
            }

            return $"{builder.ToString()}:GetAll";
        }
    }
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
