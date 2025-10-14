namespace StockFlow.Service.DTOs.User;

public class UserAuthResponseDTO
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string AccountCreatedOn { get; set; } = string.Empty;
}
