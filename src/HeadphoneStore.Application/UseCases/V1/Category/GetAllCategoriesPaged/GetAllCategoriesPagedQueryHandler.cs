using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Category;
using HeadphoneStore.Domain.Abstracts.Repositories;

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
        var result = await _categoryRepository.GetAllCategoriesPagination(
            keyword: request.SearchTerm,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        return Result.Success(result);
    }
}
