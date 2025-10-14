using System.Linq.Expressions;

namespace StockFlow.Repository.Criteria;
public class CommonOrderingCriteria<T>
{
    public Expression<Func<T, object>> OrderExpression { get; set; } = null!;

    public bool OrderDirection { get; set; } = true;
}
