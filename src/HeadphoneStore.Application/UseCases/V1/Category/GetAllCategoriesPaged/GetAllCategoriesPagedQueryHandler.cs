using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Category;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesPaged;

public class GetAllCategoriesPagedQueryHandler : IQueryHandler<GetAllCategoriesPagedQuery, PagedResult<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesPagedQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<Result<PagedResult<CategoryDto>>> Handle(GetAllCategoriesPagedQuery request, CancellationToken cancellationToken)
    {
        var query = await _categoryRepository.GetAllCategoriesPagination(
            searchTerm: request.SearchTerm,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        var resultItems = query.Items.Select(x => new CategoryDto
        {
            Id = x.Id,
            Name = x.Name,
            Slug = x.Slug,
            Description = x.Description,
            Status = x.Status.ToString(),
            Parent = x.Parent != null ? new CategoryDto
            {
                Id = x.Parent.Id,
                Name = x.Parent.Name,
                Slug = x.Slug,
                Description = x.Parent.Description,
                Status = x.Parent.Status.ToString(),
            } : null,
        }).ToList();

        return Result.Success(new PagedResult<CategoryDto>(resultItems, query.PageIndex, query.PageSize, query.TotalCount));
    }
}
