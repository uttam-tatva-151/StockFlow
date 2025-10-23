using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StockFlow.Service.Interfaces
{
    public interface IJWTService
    {
        Task<string> GenerateAccessTokenFromRefreshTokenAsync(string refreshToken);
        ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime);
        bool ValidateRefreshTokenAsync(string refreshToken);
        Task<string> GenerateAccessTokenAsync(Guid userId, string email);
        Task<string> GenerateRefreshTokenAsync(Guid userId, string email);
    }
}
