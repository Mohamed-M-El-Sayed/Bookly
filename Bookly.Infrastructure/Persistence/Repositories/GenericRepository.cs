using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Specifications;
using Bookly.Infrastructure.Persistence.Context;
using Bookly.Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookly.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly AppDbContext _appDbContext;
    private readonly DbSet<TEntity> _dbSet;
    public GenericRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        _dbSet = _appDbContext.Set<TEntity>();
    }
    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }
    public async Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> specification
        , CancellationToken cancellationTokn = default)
    {
        return await ApplaySpecification(specification).FirstOrDefaultAsync(cancellationTokn);
    }
    public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplaySpecification(specification).ToListAsync(cancellationToken);
    }
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }
    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
    private IQueryable<TEntity> ApplaySpecification(ISpecification<TEntity> specification)
    {
        return SpecificationEvaluator<TEntity>.GenerateQuery(_dbSet, specification);
    }
    public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);

        return await query.CountAsync(cancellationToken);
    }

    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    public async Task<bool> AnyAsync(
        ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        return await ApplaySpecification(specification).AnyAsync(cancellationToken);
    }
    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _dbSet;
    }
}
