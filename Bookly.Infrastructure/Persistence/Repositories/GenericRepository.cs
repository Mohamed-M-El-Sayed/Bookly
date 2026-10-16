using Bookly.Application.Contracts.Presistence;
using Bookly.Application.Contracts.Specifications;
using Bookly.Domain.Entities.Base;
using Bookly.Infrastructure.Persistence.Context;
using Bookly.Infrastructure.Persistence.Specifications;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Bookly.Infrastructure.Persistence.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
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

    public async Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
    {
        var query = ApplaySpecification(spec);
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip(spec.Skip)
            .Take(spec.Take)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }
}
