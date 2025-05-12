using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsByProductProperties;

public sealed record GetAllBrandsByProductPropertiesQuery(List<Guid> CategoryIds) : IQuery<List<BrandDto>>
{
}
