using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;

public class GetCategoryByIdQuery : IQuery<CategoryDto>
{
    public Guid Id { get; set; }
}
