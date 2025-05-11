using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Domain.Enumerations;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

using Microsoft.EntityFrameworkCore;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrands;

public class GetAllBrandsQueryHandler : IQueryHandler<GetAllBrandsQuery, List<BrandDto>>
{
    private readonly IBrandRepository _brandRepository;

    public GetAllBrandsQueryHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<Result<List<BrandDto>>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
    {
        var result = await _brandRepository.GetAllBrands();

        return Result.Success(result);
    }
}
