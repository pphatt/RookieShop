using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllFirstLevelCategories;

public class GetAllFirstLevelCategoriesQuery : IQuery<List<CategoryDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey => $"Categories:{nameof(GetAllFirstLevelCategoriesQuery)}:GetAll";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
