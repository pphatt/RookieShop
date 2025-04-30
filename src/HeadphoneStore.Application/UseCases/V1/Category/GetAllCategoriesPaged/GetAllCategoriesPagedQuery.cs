using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesPaged;

public class GetAllCategoriesPagedQuery : PagedDto, IQuery<PagedResult<CategoryDto>>
{
}
