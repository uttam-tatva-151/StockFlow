using StockFlow.Common.Constants;
using StockFlow.Common.Utils;

namespace StockFlow.Repository.DTOs.Common;
public class SortingDTO
{
    private string? _sortExpression;

    public string? SortExpression
    {
        get => _sortExpression;
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                _sortExpression = value.CapitalizeFirstLetter();
            }
            else
            {
                _sortExpression = value;
            }
        }
    }
    public string? SortDirection { get; set; } = Constant.Ascending;
}
