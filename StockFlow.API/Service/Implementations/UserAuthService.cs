using StockFlow.Common.Exceptions;
using StockFlow.Repository.Criteria;
using StockFlow.Repository.Entities;
using StockFlow.Repository.Interfaces;
using StockFlow.service.DTOs.UserAuth;
using StockFlow.Service.DTOs.UserAuth;
using StockFlow.Service.Interfaces;
using StockFlow.Service.Record;
using Service.Common.Mappings;
using StockFlow.Common.Models;
using StockFlow.Service.DTOs.User;

namespace StockFlow.Service.Implementations;

public class UserAuthService(IUserAuthRepository repository) : GenericService<UserAuth, UserAuthDTO>(repository), IUserAuthService
{
    private readonly IUserAuthRepository _userAuthRepository = repository;
    public async Task<ApiResponse> AuthenticateUserAsync(UserAuthDTO request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse> RegisterUserAsync(NewUserDTO request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // public async Task<UserAuthDTO> GetById(int id, CancellationToken cancellationToken)
    // {
    //     UserAuth item = await GetFirstOrDefaultAsync(id, cancellationToken) ?? throw new ResourceNotFoundException(nameof(UserAuth));
    //     return MapperlyMapper.Map<UserAuth, UserAuthDTO>(item);

    // }

    // public async Task<PageResponseRecord<UserAuthDTO>> Search(UserAuthSearchDTO request, CancellationToken cancellationToken)
    // {
    //     (long count, IEnumerable<UserAuth> data) = await GetAllAsyncWithCount(new Filters<UserAuth>()
    //     {
    //         Filter = x => x.Name.Trim().ToLower().Contains(request.Name.Trim().ToLower()),
    //         Select = x => new()
    //         {
    //             Id = x.Id,
    //             Name = x.Name,
    //             Url = x.Url,
    //         }

    //     }, cancellationToken);

    //     return new(count, MapperlyMapper.Map<UserAuth, UserAuthDTO>(data));

    // }

    // public async Task Post(UserAuthDTO item, CancellationToken cancellationToken)
    // {
    //     UserAuth UserAuth = MapperlyMapper.Map<UserAuthDTO, UserAuth>(item);
    //     await AddAsync(item, false, cancellationToken);
    //     return;
    // }

    // public async Task Update(UserAuthDTO item, CancellationToken cancellationToken)
    // {
    //     UserAuth UserAuth = await GetFirstOrDefaultAsync(item.Id, cancellationToken) ?? throw new ResourceNotFoundException(nameof(UserAuth));

    //     UserAuth = MapperlyMapper.Map<UserAuthDTO, UserAuth>(item);

    //     await UpdateAsync(UserAuth, false, cancellationToken);

    //     return;

    // }

    // public async Task Delete(int id, CancellationToken cancellationToken)
    // {
    //     await RemoveAsync(id, cancellationToken);
    //     return;
    // }
}
