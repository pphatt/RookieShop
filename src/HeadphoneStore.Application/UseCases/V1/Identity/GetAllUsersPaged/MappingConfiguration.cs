using HeadphoneStore.Shared.Services.Identity.GetAllUserPaged;

namespace HeadphoneStore.Application.UseCases.V1.Identity.GetAllUsersPaged;

public static class MapperConfiguration
{
    public static GetAllUsersPagedQuery MapToQuery(this GetAllUsersPagedRequestDto dto)
        => new(dto.SearchTerm, dto.PageIndex, dto.PageSize);
}
