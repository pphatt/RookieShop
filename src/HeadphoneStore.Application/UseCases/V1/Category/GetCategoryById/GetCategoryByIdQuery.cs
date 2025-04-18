using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;

public class GetCategoryByIdQuery : IQuery<CategoryDto>
{
    public Guid Id { get; set; }
}
