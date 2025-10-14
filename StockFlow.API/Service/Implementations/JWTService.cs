using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StockFlow.Common.Constants;
using StockFlow.Common.Models;
using StockFlow.Repository.Criteria;
using StockFlow.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StockFlow.Service.Implementations
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;

        public JWTService(IOptions<AppSettings> appSettings)
        {
            _jwtSettings = appSettings.Value.JwtSettings;
        }

        public async Task<string> GenerateAccessTokenFromRefreshTokenAsync(string refreshToken)
        {
                ClaimsPrincipal principal = GetPrincipalFromToken(refreshToken, validateLifetime: true);
            string userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            string email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return GenerateJweToken(userId!.ToString(), email, _jwtSettings.AccessTokenExpiryMinutes, "access");
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId, string email)
        {
            string token = GenerateJweToken(userId.ToString(), email, _jwtSettings.RefreshTokenExpiryDays * 24 * 60, Constant.JWT.RefreshToken);

            return token;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            try
            {
                ClaimsPrincipal principal = GetPrincipalFromToken(refreshToken, validateLifetime: true);
                string tokenType = principal.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
                return tokenType == Constant.JWT.RefreshToken;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GenerateAccessTokenAsync(Guid userId, string email)
        {
            return GenerateJweToken(userId.ToString(), email, _jwtSettings.AccessTokenExpiryMinutes, Constant.JWT.AccessToken);
        }
        //public async Task RevokeRefreshTokenAsync(string refreshToken)
        //{
        //    _logger.LogWarning("RevokeRefreshToken called in stateless mode - cannot revoke token.");
        //    return Task.CompletedTask;
        //}

        public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            TokenValidationParameters validationParameters = new ()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = validateLifetime,
                TokenDecryptionKey = new SymmetricSecurityKey(key)
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException(Constant.JWT.InvalidAccessToken);
            }
        }

        #region Private Helpers
        private string GenerateJweToken(string userId, string userEmail, double expiryMinutes, string tokenType)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, userId),
                new(ClaimTypes.Email, userEmail),
                new("token_type", tokenType)
            ];

            byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

            SigningCredentials signingCredentials = new (
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );

            EncryptingCredentials encryptingCredentials = new (
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.Aes256KW,
                SecurityAlgorithms.Aes256CbcHmacSha512
            );

            SecurityTokenDescriptor tokenDescriptor = new ()
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(expiryMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials
            };

            JwtSecurityTokenHandler tokenHandler = new ();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
    }

}
