# Dependency

This library was created by .Net 7.0

## Install

```bash
dotnet add package EntityFrameworkCore.GenericRepository.Nuget
```

## Create Repository

```CSharp
public selead IUserRepository : IRepository<User>
```

```CSharp
public selead UserRepository : Repository<User, AppDbContext>, IUserRepository
```

## Use
```Csharp
public selead UserService: IUserService

private readonly IUserRepository _userRepository;
private readonly IUnitOfWork _unitOfWork;

public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
{
    _userRepository = userRepository;
    _unitOfWork = unitOfWork;
}

public async Task AddAsync(User user, CancellationToken cancellationToken)
{
    await _userRepository.AddAsync(user, cancellationToken);
    await _unitOfWork.SaveChangesAsync(cancellationToken);
}

public async Task<IList<User>> GetAllAsync(CancellationToken cancellationToken)
{
    IList<User> users = await _userRepository.GetAll().ToListAsync(cancellationToken).ConfigureAwait(false);
    return users;
}
```

## Dependency Injection
```CSharp
builder.Service.AddScoped<IUnitOfWork, UnitOfWok<AppDbContext>>();
```

## Methods

This library have two services.
IRepository, IUnitOfWork

invoke
```Csharp
public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression);    
    Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<TEntity> GetFirstAsync(CancellationToken cancellationToken = default);

    TEntity GetByExpression(Expression<Func<TEntity, bool>> expression);
    TEntity GetFirst();

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task AddRangeASync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void UpdateRange(ICollection<TEntity> entities);
    Task DeleteByIdAsync(string id);
    Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);    
    void Delete(TEntity entity);
    void DeleteRange(ICollection<TEntity> entities);

}

public class Repository<TEntity, TContext> : IRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
    private readonly TContext _context;
    private DbSet<TEntity> Entity;

    public Repository(TContext context)
    {
        _context = context;
        Entity = _context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Entity.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async Task AddRangeASync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Entity.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void Delete(TEntity entity)
    {
        Entity.Remove(entity);
    }

    public async Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        TEntity entity = await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        Entity.Remove(entity);
    }

    public async Task DeleteByIdAsync(string id)
    {
        TEntity entity = await Entity.FindAsync(id).ConfigureAwait(false);
        Entity.Remove(entity);
    }

    public void DeleteRange(ICollection<TEntity> entities)
    {
        Entity.RemoveRange(entities);
    }

    public IQueryable<TEntity> GetAll()
    {
        return Entity.AsNoTracking().AsQueryable();
    }

    public TEntity GetByExpression(Expression<Func<TEntity, bool>> expression)
    {
        TEntity entity = Entity.Where(expression).AsNoTracking().FirstOrDefault();
        return entity;
    }

    public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        TEntity entity = await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return entity;
    } 

    public TEntity GetFirst()
    {
        TEntity entity = Entity.AsNoTracking().FirstOrDefault();
        return entity;
    }

    public async Task<TEntity> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        TEntity entity = await Entity.AsNoTracking().FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        return entity;
    }

    public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> expression)
    {
        return Entity.AsNoTracking().Where(expression).AsQueryable();
    }

    public void Update(TEntity entity)
    {
        Entity.Update(entity);
    }

    public void UpdateRange(ICollection<TEntity> entities)
    {
        Entity.UpdateRange(entities);
    }
}

```

```Csharp
public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    void SaveChanges();
}

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
```