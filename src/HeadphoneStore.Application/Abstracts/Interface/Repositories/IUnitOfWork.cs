namespace HeadphoneStore.Application.Abstracts.Interface.Repositories;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
