using System.Linq.Expressions;

namespace Bookly.Application.Contracts.Specifications
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        Expression<Func<TEntity, bool>>? Criteria { get; }
        List<Expression<Func<TEntity, object>>> Includes { get; }
        Expression<Func<TEntity, object>>? OrderBy { get; set; }
        Expression<Func<TEntity, object>>? OrderByDescending { get; set; }
        void ApplyOrderBy(Expression<Func<TEntity, object>> orderBy);
        void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescending);
        List<string> IncludeStrings { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPaginationEnabled { get; }
    }
}
