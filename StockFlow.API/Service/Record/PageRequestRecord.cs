namespace StockFlow.Service.Record;

public record PageRequestRecord
{
    private int _pageNumber = 1;
    public int PageNumber
    {
        get { return _pageNumber; }
        set { _pageNumber = value <= 0 ? 1 : value; }
    }

    private int _pageSize = 10;
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value <= 0 ? 50 : value; }
    }
}