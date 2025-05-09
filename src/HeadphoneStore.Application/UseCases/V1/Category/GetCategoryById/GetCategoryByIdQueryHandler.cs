using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

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
            throw new Exceptions.Category.NotFound();

        if (category.IsDeleted)
            throw new Exceptions.Category.AlreadyDeleted();

        var result = new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            Slug = category.Slug,
            Description = category.Description,
            Status = category.Status.ToString(),
            Parent = category.Parent != null ? new CategoryDto
            {
                Id = category.Parent.Id,
                Name = category.Parent.Name,
                Slug = category.Parent.Slug,
                Description = category.Parent.Description,
                Status = category.Parent.Status.ToString(),
            } : null,
            SubCategories = category.SubCategories.Select(x => new CategoryDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            })
        };

        return Result.Success(result);
    }
}
