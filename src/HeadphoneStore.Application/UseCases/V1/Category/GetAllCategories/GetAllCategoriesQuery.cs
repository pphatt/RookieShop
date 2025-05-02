using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;

public class GetAllCategoriesQuery : IQuery<List<CategoryDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey => $"Categories:{nameof(GetAllCategoriesQuery)}:GetAll";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
