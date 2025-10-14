using StockFlow.Repository.Entities.Common;

namespace StockFlow.Repository.Entities;

public class Role : AuditableEntity<Guid>
{
    public string RoleName { get; set; } = string.Empty;

    public string? Description { get; set; }
}
