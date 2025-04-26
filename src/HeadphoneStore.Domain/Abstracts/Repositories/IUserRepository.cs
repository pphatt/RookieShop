namespace HeadphoneStore.Domain.Abstracts.Repositories;

public interface IUserRepository
{
    Task<bool> UpdateLastLogin(string email);
}
