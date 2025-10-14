using StockFlow.Common.Constants;
using StockFlow.Common.Utils;
using StockFlow.Repository.Criteria;
using StockFlow.Repository.DTOs.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace StockFlow.repository.Extensions;
public static class QueryExtensions
{
    public static void CreateOrderedExpression<T>(this Filters<T> filters, SortingDTO commonSortingEntity, bool isMoreExpression = false)
    {
        if (!string.IsNullOrWhiteSpace(commonSortingEntity.SortExpression) && !string.IsNullOrWhiteSpace(commonSortingEntity.SortDirection))
        {
            commonSortingEntity.SortExpression = commonSortingEntity.SortExpression.CapitalizeFirstLetter();
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            var propertyInfo = typeof(T).GetProperty(commonSortingEntity.SortExpression);

            if (propertyInfo != null)
            {
                MemberExpression? property = Expression.Property(parameter, commonSortingEntity.SortExpression);
                Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);
                if (isMoreExpression == true)
                {
                    filters.CommonOrdering ??= [];
                    filters.CommonOrdering.Add(new CommonOrderingCriteria<T>
                    {
                        OrderExpression = lambda,
                        OrderDirection = commonSortingEntity.SortDirection != Constant.Descending
                    });
                }
                else
                {
                    if (commonSortingEntity.SortDirection == Constant.Ascending)
                    {
                        filters.OrderBy = [lambda];
                    }
                    else if (commonSortingEntity.SortDirection == Constant.Descending)
                    {
                        filters.OrderByDescending = [lambda];
                    }
                }
            }
        }
    }

    public static IQueryable<T> QueryOrdering<T>(this IQueryable<T> query, List<SortingDTO> commonSortingEntities)
    {
        commonSortingEntities = commonSortingEntities
            .Where(c => !string.IsNullOrWhiteSpace(c.SortExpression) && !string.IsNullOrWhiteSpace(c.SortDirection))
            .ToList();

        IOrderedQueryable<T>? orderedQuery = null;

        foreach (SortingDTO commonSortingEntity in commonSortingEntities)
        {
            string propertyName = commonSortingEntity.SortExpression.CapitalizeFirstLetter()!;
            ParameterExpression? parameter = Expression.Parameter(typeof(T), "x");
            PropertyInfo? propertyInfo = typeof(T).GetProperty(propertyName);

            if (propertyInfo != null)
            {
                MemberExpression? property = Expression.Property(parameter, propertyName);
                UnaryExpression? converted = Expression.Convert(property, typeof(object));
                Expression<Func<T, object>>? lambda = Expression.Lambda<Func<T, object>>(converted, parameter);
                bool ascending = commonSortingEntity.SortDirection != Constant.Descending;

                if (orderedQuery == null)
                {
                    orderedQuery = ascending ? query.OrderBy(lambda) : query.OrderByDescending(lambda);
                }
                else
                {
                    orderedQuery = ascending ? orderedQuery.ThenBy(lambda) : orderedQuery.ThenByDescending(lambda);
                }
            }
        }
        return orderedQuery ?? query;
    }

    public static List<T> CreateOrderList<T>(this List<T> data, SortingDTO commonSortingEntity)
    {
        if (!string.IsNullOrWhiteSpace(commonSortingEntity.SortExpression) && !string.IsNullOrWhiteSpace(commonSortingEntity.SortDirection))
        {
            commonSortingEntity.SortExpression = commonSortingEntity.SortExpression.CapitalizeFirstLetter();
            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            PropertyInfo? propertyInfo = typeof(T).GetProperty(commonSortingEntity.SortExpression);

            if (propertyInfo != null)
            {
                MemberExpression property = Expression.Property(parameter, commonSortingEntity.SortExpression);
                UnaryExpression converted = Expression.Convert(property, typeof(object));
                Expression<Func<T, object>> lambda = Expression.Lambda<Func<T, object>>(converted, parameter);
                bool ascending = commonSortingEntity.SortDirection != Constant.Descending;

                IEnumerable<T> orderedData = [];
                orderedData = ascending ? data.OrderBy(lambda.Compile()) : data.OrderByDescending(lambda.Compile());
                data = [.. orderedData];
            }
        }
        return data;
    }
}
