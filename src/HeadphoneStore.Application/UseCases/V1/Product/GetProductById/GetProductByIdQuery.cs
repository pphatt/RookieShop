using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>, ICacheable
{
    public bool BypassCache => false;
    public string CacheKey => $"Products:{nameof(GetProductByIdQuery)}:{Id}";
    public int SlidingExpirationInMinutes => -1;
    public int AbsoluteExpirationInMinutes => -1;
}
