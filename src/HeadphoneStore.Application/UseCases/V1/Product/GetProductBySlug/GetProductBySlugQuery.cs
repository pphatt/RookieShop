using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductBySlug;

public sealed record GetProductBySlugQuery(string Slug) : IQuery<ProductDto>
{
}
