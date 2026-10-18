using Bookly.Application.Contracts.Specifications;
using Bookly.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Bookly.Infrastructure.Persistence.Specifications;


public static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public static IQueryable<TEntity> GenerateQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
    {
        var query = inputQuery;
        // Criteria
        if (specification.Criteria is not null)
            query = query.Where(specification.Criteria);
        // Expression Includes
        foreach (var include in specification.Includes)
            query = query.Include(include);
        // String Includes
        if (specification.IncludeStrings.Any())
        {
            foreach (var includeString in specification.IncludeStrings)
                query = query.Include(includeString);
        }
        // Pagination
        if (specification.IsPaginationEnabled)
        {
            query = query
                .Skip(specification.Skip)
                .Take(specification.Take);
        }
        // Ordering
        if (specification.OrderBy is not null)
            query = query.OrderBy(specification.OrderBy);
        else if (specification.OrderByDescending is not null)
            query = query.OrderByDescending(specification.OrderByDescending);
        return query;
    }
}
