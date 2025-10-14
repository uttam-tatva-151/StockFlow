using StockFlow.Repository.Entities.Common;

namespace StockFlow.Repository.Entities;

public class UserRole : AuditableEntity<Guid>
{
    public Guid UserAuthId { get; set; }

    public Guid RoleId { get; set; }

    // Navigation properties
    public UserAuth? UserAuth { get; set; }

    public Role? Role { get; set; }
}
