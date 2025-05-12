using HeadphoneStore.Shared.Services.Product.GetAllPaged;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetAllProductsPaged;

public static class MappingConfiguration
{
    public static GetAllProductsPagedQuery MapToQuery(this GetAllProductsPagedRequestDto dto)
        => new(dto.CategoryIds, dto.SearchTerm, dto.PageIndex, dto.PageSize);
}
