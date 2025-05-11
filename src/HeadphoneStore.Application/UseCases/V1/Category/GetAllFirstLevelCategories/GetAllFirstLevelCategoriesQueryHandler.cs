using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllFirstLevelCategories;

public class GetAllFirstLevelCategoriesQueryHandler : IQueryHandler<GetAllFirstLevelCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllFirstLevelCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetAllFirstLevelCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _categoryRepository.GetAllFirstLevelCategories();

        return Result.Success(result);
    }
}
