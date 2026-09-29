using Bookly.Domain.Entities.Base;

namespace Bookly.Application.Contracts.Presistence;

public interface IUnitOfWork
{
    public IGenericRepository<TEntity> Repository<TEntity>()
           where TEntity : BaseEntity;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
