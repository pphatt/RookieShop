using AutoMapper;

using HeadphoneStore.Application.UseCases.V1.Identity.Login;
using HeadphoneStore.Application.UseCases.V1.Identity.Register;
using HeadphoneStore.Contract.Services.Identity.Login;
using HeadphoneStore.Contract.Services.Identity.Register;

namespace HeadphoneStore.Application.Mapper;

public class MapperProfiles : Profile
{
    public MapperProfiles()
    {
        // Authentication.
        CreateMap<LoginRequestDto, LoginCommand>();
        CreateMap<RegisterRequestDto, RegisterCommand>();
    }
}
