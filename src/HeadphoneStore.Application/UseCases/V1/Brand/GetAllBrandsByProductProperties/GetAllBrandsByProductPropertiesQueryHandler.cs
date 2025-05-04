using HeadphoneStore.Domain.Abstracts.Repositories;
using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsByProductProperties;

public class GetAllBrandsByProductPropertiesQueryHandler : IQueryHandler<GetAllBrandsByProductPropertiesQuery, List<BrandDto>>
{
    private readonly IBrandRepository _brandRepository;

    public GetAllBrandsByProductPropertiesQueryHandler(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<Result<List<BrandDto>>> Handle(GetAllBrandsByProductPropertiesQuery request, CancellationToken cancellationToken)
    {
        var query = await _brandRepository.GetBrandsFilteredByProductProperties(
            categoryIds: request.CategoryIds
        );

        return Result.Success(query);
    }
}
