﻿using System.Linq.Expressions;

namespace GenericRepository;

public interface IRepository<TEntity>
    where TEntity : class
{
    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAllWithTracking();
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    IQueryable<TEntity> WhereWithTracking(Expression<Func<TEntity, bool>> expression);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default, bool isTrackingActive = true);
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

}
