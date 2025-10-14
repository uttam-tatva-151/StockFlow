using StockFlow.Repository.Criteria;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StockFlow.repository.Extensions;
public static class ExpressionEvaluator
{
    public static async Task<(long count, IEnumerable<T> data)> EvaluatePageQuery<T>(this IQueryable<T> query,
        Filters<T> criteria,
        CancellationToken cancellationToken = default) where T : class
    {
        query = query.IncludeExpressions(criteria.IncludeExpressions);
        query = query.FilterQuery(criteria);

        long count = await query.CountAsync(cancellationToken);

        query = ApplyOrdering(query, criteria);

        if (criteria.IsPageRequest)
            query = query.ApplyQuery(criteria.PageNumber, criteria.PageSize);

        return (count, query.ApplySelect(criteria.Select));
    }

    private static IQueryable<T> ApplyOrdering<T>(IQueryable<T> query,
        Filters<T> criteria)
        where T : class
    {
        if (criteria.OrderBy is not null)
            query = OrderBy(query, criteria.OrderBy);

        if (criteria.OrderByDescending is not null)
            query = OrderByDesc(query, criteria.OrderByDescending);

        if (criteria.CommonOrdering is not null)
            query = CommonOrderingExpression(query, criteria.CommonOrdering);

        return query;
    }

    private static IQueryable<T> OrderBy<T>(IQueryable<T> query,
        IEnumerable<Expression<Func<T, object>>> orderByExpressions)
     where T : class
    {
        query = query.OrderBy(orderByExpressions.ElementAt(0));
        int count = orderByExpressions.Count();

        for (int i = 1; i < count; ++i)
        {
            Expression<Func<T, object>> thenBy = orderByExpressions.ElementAt(i);
            query = ((IOrderedQueryable<T>)query)
                .ThenBy(thenBy);
        }

        return query;
    }

    private static IQueryable<T> OrderByDesc<T>(IQueryable<T> query,
        IEnumerable<Expression<Func<T, object>>> orderByDescExpressions)
     where T : class
    {
        query = query.OrderByDescending(orderByDescExpressions.ElementAt(0));
        int count = orderByDescExpressions.Count();

        for (int i = 1; i < count; ++i)
        {
            Expression<Func<T, object>> thenBy = orderByDescExpressions.ElementAt(i);
            query = ((IOrderedQueryable<T>)query)
                .ThenByDescending(thenBy);
        }

        return query;
    }

    private static IQueryable<T> CommonOrderingExpression<T>(IQueryable<T> query,
        IEnumerable<CommonOrderingCriteria<T>> orderingExpressions)
     where T : class
    {
        if (orderingExpressions.Any())
        {
            if (orderingExpressions.ElementAt(0).OrderDirection)
            {
                query = query.OrderBy(orderingExpressions.ElementAt(0).OrderExpression);
            }
            else
            {
                query = query.OrderByDescending(orderingExpressions.ElementAt(0).OrderExpression);
            }
            int count = orderingExpressions.Count();

            for (int i = 1; i < count; ++i)
            {
                Expression<Func<T, object>> thenExpression = orderingExpressions.ElementAt(i).OrderExpression;
                if (orderingExpressions.ElementAt(i).OrderDirection)
                {
                    query = ((IOrderedQueryable<T>)query).ThenBy(thenExpression);
                }
                else
                {
                    query = ((IOrderedQueryable<T>)query).ThenByDescending(thenExpression);
                }
            }
        }
        return query;
    }

    public static IEnumerable<T> ApplySelect<T>(this IQueryable<T> query,
        Expression<Func<T, T>>? select = null) where T : class
        => select is null ? query.AsEnumerable() : query.Select(select).AsEnumerable();

    public static IQueryable<T> FilterQuery<T>(this IQueryable<T> query,
        Filters<T> criteria) where T : class
    {
        if (criteria.StatusFilter is not null) query = query.Where(criteria.StatusFilter);

        if (criteria.Filter is not null) query = query.Where(criteria.Filter);
        return query;
    }

    public static IQueryable<T> FilterQuery<T>(this IQueryable<T> query,
        Expression<Func<T, bool>>? filter = null) where T : class
    {
        if (filter is not null) query = query.Where(filter);
        return query;
    }

    public static IQueryable<T> IncludeExpressions<T>(this IQueryable<T> query,
        List<Expression<Func<T, object>>>? includeExpressions = null) where T : class
    {
        if (includeExpressions is null) return query;

        query = includeExpressions.Aggregate(query, (current, include) =>
        {
            return current.Include(include);
        });

        return query;
    }

    public static IQueryable<T> EvaluateQuery<T>(this IQueryable<T> query,
        Filters<T> criteria) where T : class
    {
        query = query.IncludeExpressions(criteria.IncludeExpressions);
        query = query.FilterQuery(criteria.Filter);
        query = query.FilterQuery(criteria.StatusFilter);
        query = ApplyOrdering(query, criteria);

        if (criteria.IsPageRequest)
            query = query.ApplyQuery(criteria.PageNumber, criteria.PageSize);

        return criteria.Select is not null ? query.Select(criteria.Select) : query;
    }

    public static IQueryable<T> ApplyQuery<T>(this IQueryable<T> query,
        int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        return
            query
            .Skip(skip)
            .Take(pageSize);
    }
}
