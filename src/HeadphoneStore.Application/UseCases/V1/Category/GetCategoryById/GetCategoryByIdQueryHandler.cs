using HeadphoneStore.Application.Abstracts.Interface.Repositories;
using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;

using Exceptions = Domain.Exceptions.Exceptions;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository
            .GetQueryableSet()
            .AsNoTracking()
            .Where(x => x.Id.Equals(request.Id))
            .Include(x => x.Parent)
            .Include(x => x.SubCategories)
            .SingleOrDefaultAsync();

        if (category is null)
        {
            throw new Exceptions.Category.NotFound();
        }

        if (category.IsDeleted)
        {
            throw new Exceptions.Category.AlreadyDeleted();
        }

        var result = new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedBy = category.CreatedBy,
            UpdatedBy = category.UpdatedBy,
            Parent = category.Parent != null ? new CategoryDto
            {
                Id = category.Parent.Id,
                Name = category.Parent.Name,
                Description = category.Parent.Description,
                CreatedBy = category.Parent.CreatedBy,
                UpdatedBy = category.Parent.UpdatedBy,
            } : null,
            SubCategories = category.SubCategories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
            })
        };

        return Result.Success(result);
    }
}
