using HeadphoneStore.Shared.Services.Category.GetAllCategories;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategories;

public static class MapperConfiguration
{
    public static GetAllCategoriesQuery MapToQuery(this GetAllCategoriesRequestDto dto)
        => new(dto.SearchTerm);
}
