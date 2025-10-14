using StockFlow.Repository.Entities.Common;

namespace StockFlow.Repository.Entities;

public class UserAuth : AuditableEntity<Guid>
{
    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; } 
}
