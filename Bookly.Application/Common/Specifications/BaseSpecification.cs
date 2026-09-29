using System.Linq.Expressions;
using Bookly.Application.Contracts.Specifications;
using Bookly.Domain.Entities.Base;

namespace Bookly.Application.Common.Specifications;

public class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : BaseEntity
{
    public Expression<Func<TEntity, bool>>? Criteria { get; set; }

    public List<Expression<Func<TEntity, object>>> Includes { get; } = new();
    public List<string> IncludeStrings { get; } = new();
    public int Take { get; set; }

    public int Skip { get; set; }

    public bool IsPaginationEnabled { get; set; }

    public void AddInclude(
         Expression<Func<TEntity, object>> include)
    {
        Includes.Add(include);
    }

    public void AddInclude(string include)
    {
        IncludeStrings.Add(include);
    }

    public void ApplyPagination(int pageIndex, int pageSize)
    {
        Skip = (pageIndex - 1) * pageSize;
        Take = pageSize;
        IsPaginationEnabled = true;
    }
}
