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
            Guid userId = Guid.Parse(principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            string email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            return await GenerateAccessTokenAsync(userId, email);
        }
        public async Task<string> GenerateAccessTokenAsync(Guid userId, string email)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Email, email),
                new("token_type", Constant.JWT.AccessToken)
            ];

            SymmetricSecurityKey key = new (Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            SigningCredentials creds = new (key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token)??string.Empty;
        }
        public async Task<string> GenerateRefreshTokenAsync(Guid userId, string email)
        {
            List<Claim> claims =
            [
                new(ClaimTypes.NameIdentifier, userId.ToString()),
                new(ClaimTypes.Email, email),
                new("token_type", Constant.JWT.RefreshToken)
            ];

            byte[] key = Encoding.UTF8.GetBytes(_jwtSettings.EncryptingCredentialsKey);

            SigningCredentials signingCredentials = new(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256
            );

            EncryptingCredentials encryptingCredentials = new(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.Aes256KW,
                SecurityAlgorithms.Aes256CbcHmacSha512
            );

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpiryDays),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials
            };

            JwtSecurityTokenHandler tokenHandler = new();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token) ?? string.Empty;
        }

        public bool ValidateRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return false;

            try
            {
                ClaimsPrincipal principal = GetPrincipalFromToken(refreshToken, validateLifetime: true);
                string tokenType = principal.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
                string userId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                string email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                bool isValid = tokenType == Constant.JWT.RefreshToken && !string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(email);
                return isValid;
            }
            catch
            {
                return false;
            }
        }

        //public async Task RevokeRefreshTokenAsync(string refreshToken)
        //{
        //    _logger.LogWarning("RevokeRefreshToken called in stateless mode - cannot revoke token.");
        //    return Task.CompletedTask;
        //}

        public ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifetime)
        {
            JwtSecurityTokenHandler tokenHandler = new();
            byte[] signingKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            byte[] decryptingKey = Encoding.UTF8.GetBytes(_jwtSettings.EncryptingCredentialsKey);

            TokenValidationParameters validationParameters = new ()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero,
                ValidateLifetime = validateLifetime,
                TokenDecryptionKey = new SymmetricSecurityKey(decryptingKey)
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                throw new SecurityTokenException(Constant.JWT.InvalidAccessToken);
            }
        }
    }

}
