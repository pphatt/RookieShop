using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductById;

public class GetProductByIdQuery : IQuery<ProductDto>
{
    public Guid Id { get; set; }
}
