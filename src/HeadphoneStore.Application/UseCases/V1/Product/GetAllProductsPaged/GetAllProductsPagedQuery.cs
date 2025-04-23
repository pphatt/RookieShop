using HeadphoneStore.Contract.Abstracts.Queries;
using HeadphoneStore.Contract.Abstracts.Shared;
using HeadphoneStore.Contract.Dtos.Product;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;

public class GetAllProductsPagedQuery : PagedDto, IQuery<PagedResult<ProductDto>>
{
}
