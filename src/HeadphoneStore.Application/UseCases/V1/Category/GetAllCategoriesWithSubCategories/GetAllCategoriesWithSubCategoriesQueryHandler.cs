using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesWithSubCategories;

public class GetAllCategoriesWithSubCategoriesQueryHandler : IQueryHandler<GetAllCategoriesWithSubCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesWithSubCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesWithSubCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetAllCategoriesWithSubCategories(request.CategorySlug);

        return Result.Success(result);
    }
}
