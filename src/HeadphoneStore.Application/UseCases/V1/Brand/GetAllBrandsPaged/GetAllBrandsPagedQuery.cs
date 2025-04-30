using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Abstracts.Shared;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;

public class GetAllBrandsPagedQuery : PagedDto, IQuery<PagedResult<BrandDto>>
{
}
