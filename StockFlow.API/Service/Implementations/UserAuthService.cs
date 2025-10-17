using StockFlow.Repository.Criteria;
using StockFlow.Repository.Entities;
using StockFlow.Repository.Interfaces;
using StockFlow.Service.DTOs.UserAuth;
using StockFlow.Service.Interfaces;
using StockFlow.Common.Models;
using StockFlow.Service.DTOs.User;
using StockFlow.Common.Constants;

namespace StockFlow.Service.Implementations;

public class UserAuthService(IUserAuthRepository repository, IJWTService jwtService) : GenericService<UserAuth, UserAuthDTO>(repository), IUserAuthService
{
    private readonly IUserAuthRepository _userAuthRepository = repository;
    private readonly IJWTService _jwtService = jwtService;
    public async Task<ApiResponse> AuthenticateUserAsync(UserAuthDTO request, CancellationToken cancellationToken)
    {
        try
        {
            // Fetch user
            UserAuth user = await GetUserByEmailAsync(request.Email, cancellationToken);
            if (user == null)
                return ApiResponse.FailResponse(Constant.ErrorMessages.InvalidCredentials);

            // Validate password
            if (!VerifyPassword(request.Password, user.PasswordHash))
                return ApiResponse.FailResponse(Constant.ErrorMessages.InvalidCredentials);

            // Generate token
            var token = _jwtService.GenerateAccessTokenAsync(user.Id, user.Email);

            UserAuthResponseDTO result = new()
            {
                UserId = user.Id,
                Email = user.Email,
                AccountCreatedOn = user.CreatedDate.ToString(),
            };

            return ApiResponse.SuccessResponse(Constant.SuccessMessages.LoginSuccessfully, result);
        }
        catch (Exception ex)
        {
            return ApiResponse.FailResponse($"Authentication failed: {ex.Message}");
        }
    }


    public async Task<ApiResponse> RegisterUserAsync(NewUserDTO request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if user already exists
            UserAuth existingUser = await GetUserByEmailAsync(request.EmailId, cancellationToken);
            if (existingUser != null)
                return ApiResponse.FailResponse(Constant.WarningMessages.EmailAlreadyExists);

            // Hash password
            string hashedPassword = HashPassword(request.Password);

            // Create new entity
            UserAuth newUser = new()
            {
                Email = request.EmailId,
                PasswordHash = hashedPassword,
                LastLoginAt = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
            };

            await AddAsync(newUser, false, cancellationToken);

            object result = new { newUser.Id, newUser.Email };

            return ApiResponse.SuccessResponse(Constant.SuccessMessages.RegisterSuccessfully, result);
        }
        catch (Exception ex)
        {
            return ApiResponse.FailResponse($"Registration failed: {ex.Message}");
        }
    }

    // -----------------------------
    // PRIVATE HELPER METHODS
    // -----------------------------
    private async Task<UserAuth?> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _userAuthRepository.GetFirstOrDefaultAsync(
            new Filters<UserAuth>() { Filter = u => u.Email == email && u.IsActive },
            cancellationToken
        );
    }

    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private static bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
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
