using Bookly.Application.Contracts.Presistence;
using Bookly.Infrastructure.Persistence.Context;
using Bookly.Infrastructure.Persistence.Repositories;

namespace Bookly.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<Type, object> _repositories = new();
        private readonly AppDbContext _appDbContext;
        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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

    }
}
