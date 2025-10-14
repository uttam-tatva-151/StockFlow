namespace StockFlow.Repository.Entities.Common;

public class BaseEntity
{ }

public class Entity<T> : BaseEntity where T : struct
{
    public T Id { get; set; }
}
