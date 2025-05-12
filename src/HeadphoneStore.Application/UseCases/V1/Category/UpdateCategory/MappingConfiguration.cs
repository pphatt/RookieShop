using HeadphoneStore.Shared.Services.Category.Update;

namespace HeadphoneStore.Application.UseCases.V1.Category.UpdateCategory;

public static class MappingConfiguration
{
    public static UpdateCategoryCommand MapToCommand(this UpdateCategoryRequestDto dto)
        => new(dto.Id, dto.Name, dto.Slug, dto.Description, dto.ParentCategoryId, dto.Status);
}
