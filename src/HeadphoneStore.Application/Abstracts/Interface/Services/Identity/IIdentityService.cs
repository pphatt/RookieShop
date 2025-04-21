namespace HeadphoneStore.Application.Abstracts.Interface.Services.Identity;

public interface IIdentityService
{
    Guid GetUserId();
    bool? IsAuthenticated();
}
