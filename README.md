# Dependency

This library was created by .Net 8.0

## Install
```bash
dotnet add package EntityFrameworkCore.GenericRepository.Nuget
```

## UnitOfWork Implementation
```CSharp
public class ApplicationDbContext : IUnitOfWork
```

## Create Repository
```CSharp
public selead IUserRepository : IRepository<User>
```

```CSharp
public selead UserRepository : Repository<User, ApplicationDbContext>, IUserRepository
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
    IList<User> users = await _userRepository.GetAll().ToListAsync(cancellationToken);
    return users;
}
```

## Dependency Injection
```CSharp
builder.Service.AddScoped<IUserRepository, UserRepository>();
builder.Service.AddScoped<IUnitOfWork>(cfr => cfr.GetRequiredService<ApplicationDbContext>());
```

## Methods
This library have two services.
IRepository, IUnitOfWork

```Csharp
public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAllWithTracking();
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);    
    IQueryable<TEntity> WhereWithTracking(Expression<Func<TEntity, bool>> expression);    
    Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<TEntity> GetByExpressionWithTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    Task<TEntity> GetFirstAsync(CancellationToken cancellationToken = default);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    bool Any(Expression<Func<TEntity, bool>> expression);
    TEntity GetByExpression(Expression<Func<TEntity, bool>> expression);
    TEntity GetByExpressionWithTracking(Expression<Func<TEntity, bool>> expression);
    TEntity GetFirst();
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Add(TEntity entity);
    Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void UpdateRange(ICollection<TEntity> entities);
    Task DeleteByIdAsync(string id);
    Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);    
    void Delete(TEntity entity);
    void DeleteRange(ICollection<TEntity> entities);

}
```

```Csharp
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

    public void Add(TEntity entity)
    {
        Entity.Add(entity);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Entity.AddAsync(entity, cancellationToken);
    }

    public async Task AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await Entity.AddRangeAsync(entities, cancellationToken);
    }

    public bool Any(Expression<Func<TEntity, bool>> expression)
    {
        return Entity.Any(expression);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await Entity.AnyAsync(expression, cancellationToken);
    }

    public void Delete(TEntity entity)
    {
        Entity.Remove(entity);
    }

    public async Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        TEntity entity = await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        Entity.Remove(entity);
    }

    public async Task DeleteByIdAsync(string id)
    {
        TEntity entity = await Entity.FindAsync(id);
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

    public IQueryable<TEntity> GetAllWithTracking()
    {
        return Entity.AsQueryable();
    }

    public TEntity GetByExpression(Expression<Func<TEntity, bool>> expression)
    {
        TEntity entity = Entity.Where(expression).AsNoTracking().FirstOrDefault();
        return entity;
    }

    public async Task<TEntity> GetByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        TEntity entity = await Entity.Where(expression).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        return entity;
    }

    public TEntity GetByExpressionWithTracking(Expression<Func<TEntity, bool>> expression)
    {
        TEntity entity = Entity.Where(expression).FirstOrDefault();
        return entity;
    }

    public async Task<TEntity> GetByExpressionWithTrackingAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
    {
        TEntity entity = await Entity.Where(expression).FirstOrDefaultAsync(cancellationToken);
        return entity;
    }

    public TEntity GetFirst()
    {
        TEntity entity = Entity.AsNoTracking().FirstOrDefault();
        return entity;
    }

    public async Task<TEntity> GetFirstAsync(CancellationToken cancellationToken = default)
    {
        TEntity entity = await Entity.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        return entity;
    }

    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        return Entity.AsNoTracking().Where(expression).AsQueryable();
    }

    public IQueryable<TEntity> WhereWithTracking(Expression<Func<TEntity, bool>> expression)
    {
        return Entity.Where(expression).AsQueryable();
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
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    int SaveChanges();
}
```
