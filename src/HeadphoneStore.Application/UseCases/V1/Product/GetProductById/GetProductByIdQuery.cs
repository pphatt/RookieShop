using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductById;

public class GetProductByIdQuery : IQuery<ProductDto>
{
    public Guid Id { get; set; }
}
