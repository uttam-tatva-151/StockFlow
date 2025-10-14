using System.ComponentModel.DataAnnotations;

namespace StockFlow.Service.DTOs.User;

public class NewUserDTO
{
    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string EmailId { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, one special character and be at least 8 characters long.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; } = null!;
}
