using HeadphoneStore.Application.Abstracts.Interface.Repositories;
using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Brand;

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
        var result = await _brandRepository.GetBrandsPaging(
            keyword: request.Keyword,
            pageIndex: request.PageIndex,
            pageSize: request.PageSize
        );

        return Result.Success(result);
    }
}
