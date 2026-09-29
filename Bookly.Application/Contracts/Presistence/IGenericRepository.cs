using Bookly.Application.Contracts.Specifications;
using Bookly.Domain.Entities.Base;

namespace Bookly.Application.Contracts.Presistence;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    Task<TEntity?> GetEntityWithSpecAsync(ISpecification<TEntity> specification, CancellationToken cancellationTokn = default);
    Task<(IReadOnlyList<TEntity> Items, int TotalCount)> GetPagedAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
}
