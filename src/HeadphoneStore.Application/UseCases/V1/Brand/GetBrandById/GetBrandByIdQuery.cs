using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Dtos.Brand;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetBrandById;

public class GetBrandByIdQuery : ICommand<BrandDto>
{
    public Guid Id { get; set; }
}
