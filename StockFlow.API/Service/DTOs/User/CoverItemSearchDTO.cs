using StockFlow.Repository.DTOs.Common;

namespace StockFlow.service.DTOs.UserAuth;
public class UserAuthSearchDTO : PageRequestDTO
{
    public string Name { get; set; }
}
