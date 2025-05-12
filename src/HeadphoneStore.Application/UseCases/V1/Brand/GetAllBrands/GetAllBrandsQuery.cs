using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrands;

public sealed record GetAllBrandsQuery : IQuery<List<BrandDto>>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey => $"Brands:{nameof(GetAllBrandsQuery)}:GetAll";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
