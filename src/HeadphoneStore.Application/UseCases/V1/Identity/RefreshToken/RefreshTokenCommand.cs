using HeadphoneStore.Contract.Abstracts.Commands;
using HeadphoneStore.Contract.Services.Identity.RefreshToken;

namespace HeadphoneStore.Application.UseCases.V1.Identity.RefreshToken;

public class RefreshTokenCommand : ICommand<RefreshTokenResponseDto>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
