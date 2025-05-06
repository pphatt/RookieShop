using HeadphoneStore.Shared.Services.Identity.Login;

namespace HeadphoneStore.StoreFrontEnd.Interfaces.Services;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto model);

    Task<bool> LogoutAsync();
}