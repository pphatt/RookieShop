using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

using Microsoft.EntityFrameworkCore;

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
        var query = _categoryRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Where(x => x.Status == EntityStatus.Active && x.Slug.Contains(request.CategorySlug))
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            });

        var result = await query.ToListAsync();

        return Result.Success(result);
    }
}
