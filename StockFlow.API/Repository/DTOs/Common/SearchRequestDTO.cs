using StockFlow.Common.Constants;

namespace StockFlow.Repository.DTOs.Common;
public class SearchRequestDTO : PageRequestDTO
{
    public string? SortExpression { get; set; }
    public string? SortDirection { get; set; } = Constant.Ascending;
}
