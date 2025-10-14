namespace StockFlow.Repository.Entities.Common;

public class AuditableEntity<T> : Entity<T> where T : struct
{
    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? ModifiedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }
}

