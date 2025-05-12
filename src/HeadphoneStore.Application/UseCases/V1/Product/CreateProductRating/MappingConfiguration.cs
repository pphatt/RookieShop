using HeadphoneStore.Shared.Services.Product.CreateProductRating;

namespace HeadphoneStore.Application.UseCases.V1.Product.CreateProductRating;

public static class MappingConfiguration
{
    public static CreateProductRatingCommand MapToCommand(this CreateProductRatingRequestDto dto, Guid customerId)
        => new(dto.ProductId, customerId, dto.RatingValue, dto.Comment);
}
