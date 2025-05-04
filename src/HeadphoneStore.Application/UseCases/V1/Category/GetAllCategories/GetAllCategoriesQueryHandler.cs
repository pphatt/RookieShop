using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = _categoryRepository
            .FindAll(x => x.ParentId == null)
            .Where(x => x.Status == EntityStatus.Active);

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            categories = categories.Where(x => x.Name.Contains(request.SearchTerm));
        }

        var result = await categories
            .OrderBy(x => x.CreatedDateTime)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
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
                        CreatedBy = c.CreatedBy,
                        UpdatedBy = c.UpdatedBy,
                    })
            })
            .ToListAsync();

        return Result.Success(result);
    }
}
