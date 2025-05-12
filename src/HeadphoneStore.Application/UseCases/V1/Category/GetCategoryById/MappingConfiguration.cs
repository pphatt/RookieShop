using HeadphoneStore.Shared.Services.Category.GetCategoryById;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetCategoryById;

public static class MappingConfiguration
{
    public static GetCategoryByIdQuery MapToQuery(this GetCategoryByIdRequestDto dto)
        => new(dto.Id);
}
