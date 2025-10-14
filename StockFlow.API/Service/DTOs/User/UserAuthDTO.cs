using System.ComponentModel.DataAnnotations;

namespace StockFlow.Service.DTOs.UserAuth;
public sealed class UserAuthDTO
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(254, MinimumLength = 6, ErrorMessage = "Email must be between 6 and 254 characters.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [MaxLength(16, ErrorMessage = "Password cannot be longer than 16 characters.")]
    public string Password { get; set; } = null!;
    public bool IsRememberMe { get; set; }
}

