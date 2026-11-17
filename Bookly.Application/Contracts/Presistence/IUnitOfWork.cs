namespace Bookly.Application.Contracts.Presistence;

public interface IUnitOfWork
{
    public IGenericRepository<TEntity> Repository<TEntity>()
           where TEntity : class;
    IReviewRepository Reviews { get; }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);

}
