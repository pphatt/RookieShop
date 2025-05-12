using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Shared.Services.Brand.Create;

namespace HeadphoneStore.Application.UseCases.V1.Brand.CreateBrand;

public static class MappingConfiguration
{
    public static CreateBrandCommand MapToCommand(this CreateBrandRequestDto dto)
    {
        var slug = dto.Slug ?? dto.Name.Slugify();
        return new(dto.Name, slug, dto.Description, dto.Status);
    }
}
