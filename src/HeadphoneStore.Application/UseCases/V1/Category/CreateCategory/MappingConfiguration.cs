using HeadphoneStore.Application.DependencyInjection.Extensions;
using HeadphoneStore.Shared.Services.Category.Create;

namespace HeadphoneStore.Application.UseCases.V1.Category.CreateCategory;

public static class MappingConfiguration
{
    public static CreateCategoryCommand MapToCommand(this CreateCategoryRequestDto dto)
    {
        var slug = dto.Slug ?? dto.Name.Slugify();
        return new(dto.Name, slug, dto.Description, dto.ParentCategoryId, dto.Status);
    }
}
