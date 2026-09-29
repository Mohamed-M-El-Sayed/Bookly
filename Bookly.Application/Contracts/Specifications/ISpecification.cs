using System.Linq.Expressions;
using Bookly.Domain.Entities.Base;

namespace Bookly.Application.Contracts.Specifications
{
    public interface ISpecification<TEntity> where TEntity : BaseEntity
    {
        Expression<Func<TEntity, bool>>? Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }

        List<string> IncludeStrings { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPaginationEnabled { get; }
    }
}
