using HeadphoneStore.Shared.Services.Category.GetAllPaged;

namespace HeadphoneStore.Application.UseCases.V1.Category.GetAllCategoriesPaged;

public static class MappingConfiguration
{
    public static GetAllCategoriesPagedQuery MapToQuery(this GetAllCategoriesPagedRequestDto dto)
        => new(dto.SearchTerm, dto.PageIndex, dto.PageSize);
}
