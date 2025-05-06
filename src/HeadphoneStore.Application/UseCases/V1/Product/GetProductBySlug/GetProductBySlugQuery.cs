using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductBySlug;

public class GetProductBySlugQuery : IQuery<ProductDto>
{
    public string Slug { get; set; }
}
