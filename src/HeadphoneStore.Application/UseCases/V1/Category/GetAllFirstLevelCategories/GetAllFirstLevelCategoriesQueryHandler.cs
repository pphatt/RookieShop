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
        var categories = _categoryRepository
            .FindAll(x => x.ParentId == null)
            .Where(x => !x.IsDeleted && x.Status == EntityStatus.Active)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            });

        var result = await categories.ToListAsync();

        return Result.Success(result);
    }
}
