using HeadphoneStore.Shared.Services.Category.InactivateCategory;

namespace HeadphoneStore.Application.UseCases.V1.Category.InactivateCategory;

public static class MappingConfiguration
{
    public static InactivateCategoryCommand MapToCommand(this InactivateCategoryRequestDto dto)
        => new(dto.Id);
}
