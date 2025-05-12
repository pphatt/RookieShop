using HeadphoneStore.Shared.Services.Category.Delete;

namespace HeadphoneStore.Application.UseCases.V1.Category.DeleteCategory;

public static class MappingConfiguration
{
    public static DeleteCategoryCommand MapToCommand(this DeleteCategoryRequestDto dto)
        => new(dto.Id);
}
