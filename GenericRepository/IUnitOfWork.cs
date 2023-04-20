namespace GenericRepository;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void SaveChanges();
}
