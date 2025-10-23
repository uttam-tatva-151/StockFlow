namespace StockFlow.Repository.Entities.Common;

public class AuditableEntity<T> : Entity<T> where T : struct
{
    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;
}

