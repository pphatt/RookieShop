using HeadphoneStore.Shared.Services.Product.GetProductBySlug;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductBySlug;

public static class MapperConfiguration
{
    public static GetProductBySlugQuery MapToQuery(this GetProductBySlugRequestDto dto)
        => new(dto.Slug);
}
