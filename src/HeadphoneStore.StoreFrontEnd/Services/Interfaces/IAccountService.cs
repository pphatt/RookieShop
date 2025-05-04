using HeadphoneStore.Shared.Services.Identity.Login;

namespace HeadphoneStore.StoreFrontEnd.Services.Interfaces;

public interface IAccountService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto model);

    Task<bool> LogoutAsync();
}