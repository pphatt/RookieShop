using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Domain.Abstracts.Repositories;

using Microsoft.EntityFrameworkCore;

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
        var query = _categoryRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Include(x => x.SubCategories)
            .SelectMany(c => c.SubCategories)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
            });

        var result = await query.ToListAsync();

        return Result.Success(result);
    }
}
