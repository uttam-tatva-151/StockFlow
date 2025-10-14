using StockFlow.Common.Models;
using StockFlow.Repository.Entities;
using StockFlow.service.DTOs.UserAuth;
using StockFlow.Service.DTOs.User;
using StockFlow.Service.DTOs.UserAuth;
using StockFlow.Service.Record;

namespace StockFlow.Service.Interfaces;
public interface IUserAuthService : IGenericService<UserAuth, UserAuthDTO>
{
    // Task<UserAuthDTO> GetById(int id, CancellationToken cancellationToken);

    // Task<PageResponseRecord<UserAuthDTO>> Search(UserAuthSearchDTO request, CancellationToken cancellationToken);

    // Task Post(UserAuthDTO item, CancellationToken cancellationToken);

    // Task Update(UserAuthDTO item, CancellationToken cancellationToken);

    // Task Delete(int id, CancellationToken cancellationToken);
    Task<ApiResponse> AuthenticateUserAsync(UserAuthDTO request, CancellationToken cancellationToken);
    Task<ApiResponse> RegisterUserAsync(NewUserDTO request, CancellationToken cancellationToken);
}