using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;

public class GetAllBrandsPagedQueryHandler : IQueryHandler<GetAllBrandsPagedQuery, PagedResult<BrandDto>>
{
    private readonly IBrandRepository _brandRepository;

    public GetAllBrandsPagedQueryHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<Result<PagedResult<BrandDto>>> Handle(GetAllBrandsPagedQuery request, CancellationToken cancellationToken)
    {
        var query = await _brandRepository.GetBrandsPagination(
            keyword: request.SearchTerm,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        var resultItem = query.Items
            .Select(x => new BrandDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            }).ToList();

        return Result.Success(new PagedResult<BrandDto>(resultItem, query.PageIndex, query.PageSize, query.TotalCount));
    }
}
