using HeadphoneStore.Shared.Services.Category.ActivateCategory;

namespace HeadphoneStore.Application.UseCases.V1.Category.ActivateCategory;

public static class MappingConfiguration
{
    public static ActivateCategoryCommand MapToCommand(this ActivateCategoryRequestDto dto)
        => new(dto.Id);
}
