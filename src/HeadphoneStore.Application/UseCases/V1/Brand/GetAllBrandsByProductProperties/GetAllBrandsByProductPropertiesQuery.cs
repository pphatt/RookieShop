using HeadphoneStore.Shared.Abstracts.Queries;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsByProductProperties;

public class GetAllBrandsByProductPropertiesQuery : IQuery<List<BrandDto>>
{
    public List<Guid> CategoryIds { get; set; }
}
