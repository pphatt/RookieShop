using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllSubCategories;

public class GetAllSubCategoriesQueryHandler : IQueryHandler<GetAllSubCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllSubCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetAllSubCategoriesQuery request, CancellationToken cancellationToken)
    {
        var query = await _categoryRepository.GetAllSubCategories();

        var result = query
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            })
            .ToList();

        return Result.Success(result);
    }
}
