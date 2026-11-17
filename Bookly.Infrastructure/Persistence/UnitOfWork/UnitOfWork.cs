using Bookly.Application.Contracts.Presistence;
using Bookly.Infrastructure.Persistence.Context;
using Bookly.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bookly.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly AppDbContext _appDbContext;
        private IDbContextTransaction? _transaction;
        public IReviewRepository Reviews { get; }
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            Reviews = new ReviewRepository(_appDbContext);
        }
        public IGenericRepository<TEntity> Repository<TEntity>()
            where TEntity : class
        {
            var type = typeof(TEntity);
            if (!_repositories.TryGetValue(type, out var repository))
            {
                repository = new GenericRepository<TEntity>(_appDbContext);
                _repositories[type] = repository;
            }
            return (IGenericRepository<TEntity>)repository;
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _appDbContext.SaveChangesAsync(cancellationToken);
        public async Task BeginTransactionAsync(
      CancellationToken cancellationToken = default)
        {
            if (_transaction is not null)
                throw new InvalidOperationException(
                    "A transaction is already active.");

            _transaction = await _appDbContext.Database.BeginTransactionAsync(
                cancellationToken);
        }

        public async Task CommitTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                throw new InvalidOperationException(
                    "No active transaction.");

            try
            {
                await _transaction.CommitAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync(
            CancellationToken cancellationToken = default)
        {
            if (_transaction is null)
                return;
            try
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }
}
