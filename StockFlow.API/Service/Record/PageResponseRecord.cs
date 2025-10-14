namespace StockFlow.Service.Record;

public sealed record PageResponseRecord<T>
    (
        long Count,
        IEnumerable<T> Data
    );
