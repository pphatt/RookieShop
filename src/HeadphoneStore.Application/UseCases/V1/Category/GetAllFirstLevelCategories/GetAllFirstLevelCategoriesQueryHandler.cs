using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

using Microsoft.EntityFrameworkCore;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        var query = await _categoryRepository.GetAllFirstLevelCategories();

        var result = query
            .OrderBy(x => x.CreatedDateTime)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
                SubCategories = x.SubCategories
                    .Where(x => x.Status == EntityStatus.Active)
                    .OrderBy(x => x.CreatedDateTime)
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        Status = c.Status.ToString(),
                    })
            })
            .ToList();

        return Result.Success(result);
    }
}
