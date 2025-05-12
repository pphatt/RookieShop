using HeadphoneStore.Shared.Services.Product.GetById;

namespace HeadphoneStore.Application.UseCases.V1.Product.GetProductById;

public static class MapperConfiguration
{
    public static GetProductByIdQuery MapToQuery(this GetProductByIdRequestDto dto)
        => new(dto.Id);
}
