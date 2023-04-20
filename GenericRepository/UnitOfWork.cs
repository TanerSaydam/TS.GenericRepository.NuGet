using Microsoft.EntityFrameworkCore;

namespace GenericRepository;

public sealed class UnitOfWork<TContext> : IUnitOfWork
    where TContext: DbContext
{
    private readonly TContext _context;

    public UnitOfWork(TContext context)
    {
        _context = context;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
