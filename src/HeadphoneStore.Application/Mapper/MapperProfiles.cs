using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;

namespace HeadphoneStore.Application.Mapper;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        // Authentication.
        CreateMap<LoginRequest, LoginCommand>();
    }
}
