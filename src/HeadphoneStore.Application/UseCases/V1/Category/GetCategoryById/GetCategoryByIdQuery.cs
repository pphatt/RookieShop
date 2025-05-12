using HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryDto>
{
    public bool BypassCache => false;
    public string CacheKey => $"Categories:{nameof(GetCategoryByIdQuery)}:{Id}";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
