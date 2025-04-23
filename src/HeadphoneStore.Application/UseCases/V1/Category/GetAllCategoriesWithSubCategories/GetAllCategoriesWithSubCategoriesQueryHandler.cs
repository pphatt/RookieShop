using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Domain.Abstracts.Repositories;

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
        var query =_categoryRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Include(x => x.SubCategories)
            .Include(x => x.Parent)
            .Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                Parent = x.Parent != null ? new CategoryDto
                {
                    Id = x.Parent.Id,
                    Name = x.Parent.Name,
                    Description = x.Parent.Description,
                    CreatedBy = x.Parent.CreatedBy,
                    UpdatedBy = x.Parent.UpdatedBy,
                } : null,
                SubCategories = x.SubCategories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedBy = c.CreatedBy,
                    UpdatedBy = c.UpdatedBy,
                })
            });

        var result = await query.ToListAsync();

        return Result.Success(result);
    }
}
