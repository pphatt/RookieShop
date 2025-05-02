using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;

public class GetBrandByIdQuery : IQuery<BrandDto>, ICacheable
{
    public Guid Id { get; set; }

    public bool BypassCache => false;
    public string CacheKey => $"Brands:{nameof(GetBrandByIdQuery)}:{Id}";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
