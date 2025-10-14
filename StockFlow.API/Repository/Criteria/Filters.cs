using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace StockFlow.Repository.Criteria;

/// <summary>
/// Represents criteria for filtering and paging a collection of items of type T.
/// </summary>
/// <typeparam name="T">The type of items to filter and page.</typeparam>
public class Filters<T>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public bool IsPageRequest { get; set; } = false;

    /// <summary>
    /// Gets or sets the filter expression to apply when querying the collection of items.
    /// </summary>
    public Expression<Func<T, bool>>? Filter { get; set; } = null;

    /// <summary>
    /// Gets or sets a list of expressions specifying related entities to include in the query.
    /// </summary>
    public List<Expression<Func<T, object>>>? IncludeExpressions { get; set; } = null;

    /// <summary>
    /// Gets or sets the status filter expression to apply when querying the collection of items.
    /// </summary>
    public Expression<Func<T, bool>>? StatusFilter { get; set; } = null;

    /// <summary>
    /// Gets or sets the expression to select specific properties of items in the query results.
    /// </summary>
    public Expression<Func<T, T>>? Select { get; set; } = null;

    /// <summary>
    /// Gets or sets the expression to order the query results by a specified property in ascending order.
    /// </summary>
    public List<Expression<Func<T, object>>>? OrderBy { get; set; } = null;

    /// <summary>
    /// Gets or sets the expression to order the query results by a specified property in descending order.
    /// </summary>
    public List<Expression<Func<T, object>>>? OrderByDescending { get; set; } = null;

    /// <summary>
    /// Gets or sets the expression to order the query results by a specified order.
    /// </summary>
    public List<CommonOrderingCriteria<T>>? CommonOrdering { get; set; } = null;

    /// <summary>
    /// Gets or sets the expression to order the query results by a specified property in descending order.
    /// </summary>
    public Func<T, object>? GroupBy { get; set; } = null;

    /// <summary>
    /// Gets or sets the expression to update specified properties.
    /// </summary>
    public Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>>? UpdateExpression { get; set; } = null;

}