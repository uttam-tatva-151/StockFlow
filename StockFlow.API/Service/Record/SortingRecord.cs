using StockFlow.Common.Constants;

namespace StockFlow.Service.Record;
public record SortingRecord
{
    public string? SortExpression { get; init; }
    public string? SortDirection { get; init; } = Constant.Ascending;
}
