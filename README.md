# Dependency

This library was created by .Net 9.0

## Install
```bash
dotnet add package TS.EntityFrameworkCore.GenericRepository
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

public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
{
    User? user = await _userRepository.FirstOrDefaultAsync(p=> p.Id == id, cancellationToken);
    return user;
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
builder.Services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());
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
    TEntity First(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true);
    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression, bool isTrackingActive = true);
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default, bool isTrackingActive = true);
    Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default, bool isTrackingActive = true);
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
    void AddRange(ICollection<TEntity> entities);
    void Update(TEntity entity);
    void UpdateRange(ICollection<TEntity> entities);
    Task DeleteByIdAsync(string id);
    Task DeleteByExpressionAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    void Delete(TEntity entity);
    void DeleteRange(ICollection<TEntity> entities);
    IQueryable<KeyValuePair<bool, int>> CountBy(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);    
    int Count();
    int Count(Expression<Func<TEntity, bool>> expression);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
}
```