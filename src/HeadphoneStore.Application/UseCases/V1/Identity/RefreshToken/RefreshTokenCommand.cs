using HeadphoneStore.Shared.Abstracts.Commands;
using HeadphoneStore.Shared.Services.Identity.RefreshToken;

namespace HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;

public class RefreshTokenCommand : ICommand<RefreshTokenResponseDto>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
