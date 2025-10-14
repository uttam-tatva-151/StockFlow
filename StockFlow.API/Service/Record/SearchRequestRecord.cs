using StockFlow.Common.Constants;
using StockFlow.Service.Record;

namespace StockFlow.service.Record;
public record SearchRequestRecord : PageRequestRecord
{
    public string? SortExpression { get; init; }
    public string? SortDirection { get; init; } = Constant.Ascending;
}
