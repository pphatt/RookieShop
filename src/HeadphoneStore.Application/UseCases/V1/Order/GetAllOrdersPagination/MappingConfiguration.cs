using HeadphoneStore.Shared.Services.Order.GetAllOrdersPagination;

namespace HeadphoneStore.Application.UseCases.V1.Order.GetAllOrdersPagination;

public static class MappingConfiguration
{
    public static GetAllOrdersPaginationQuery MapToQuery(this GetAllOrdersPaginationRequestDto dto, Guid customerId)
        => new(customerId, dto.SearchTerm, dto.PageIndex, dto.PageSize);
}
