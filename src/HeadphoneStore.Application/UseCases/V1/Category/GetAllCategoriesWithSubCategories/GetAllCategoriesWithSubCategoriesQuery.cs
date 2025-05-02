using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesWithSubCategories;

public class GetAllCategoriesWithSubCategoriesQuery : IQuery<List<CategoryDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey => $"Categories:{nameof(GetAllCategoriesWithSubCategoriesQuery)}:GetAll";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
