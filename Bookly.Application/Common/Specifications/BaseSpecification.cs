using Bookly.Application.Contracts.Specifications;
using System.Linq.Expressions;

namespace Bookly.Application.Common.Specifications;

public class BaseSpecification<TEntity> : ISpecification<TEntity>
    where TEntity : class
{
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();

    public List<string> IncludeStrings { get; } = new();

    public Expression<Func<TEntity, object>>? OrderBy { get; set; }

    public Expression<Func<TEntity, object>>? OrderByDescending { get; set; }

    public int Take { get; set; }

    public int Skip { get; set; }

    public bool IsPaginationEnabled { get; set; }

    public void AddInclude(Expression<Func<TEntity, object>> include)
    {
        Includes.Add(include);
    }

    public void AddInclude(string include)
    {
        IncludeStrings.Add(include);
    }

    public void ApplyOrderBy(Expression<Func<TEntity, object>> orderBy)
    {
        OrderBy = orderBy;
    }

    public void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
    {
        OrderByDescending = orderByDescending;
    }

    public void ApplyPagination(int pageIndex, int pageSize)
    {
        Skip = (pageIndex - 1) * pageSize;
        Take = pageSize;
        IsPaginationEnabled = true;
    }
}