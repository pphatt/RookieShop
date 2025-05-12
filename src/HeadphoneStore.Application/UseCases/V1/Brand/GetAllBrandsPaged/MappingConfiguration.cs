using HeadphoneStore.Shared.Services.Brand.GetAllPaged;

namespace HeadphoneStore.Application.UseCases.V1.Brand.GetAllBrandsPaged;

public static class MappingConfiguration
{
    public static GetAllBrandsPagedQuery MapToQuery(this GetAllBrandsPagedRequestDto dto)
        => new(dto.SearchTerm, dto.PageIndex, dto.PageSize);
}
