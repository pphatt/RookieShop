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
        var query = await _brandRepository.GetAllBrands();

        var result = query
            .Select(x => new BrandDto
            {
                Id = x.Id,
                Name = x.Name,
                Slug = x.Slug,
                Description = x.Description,
                Status = x.Status.ToString(),
            }).ToList();

        return Result.Success(result);
    }
}
