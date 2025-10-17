using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockFlow.API.Middlewares;
using StockFlow.Common.Constants;
using StockFlow.Service.Interfaces;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace StockFlow.API;

public class JWTMiddleware(RequestDelegate next, ILogger<JWTMiddleware> logger)
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JWTMiddleware> _logger;
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            IJWTService jwtService = context.RequestServices.GetRequiredService<IJWTService>();
            string? accessToken = ExtractAccessToken(context);
            string? refreshToken = ExtractRefreshToken(context);

            await HandleTokenValidationAsync(context, jwtService, accessToken, refreshToken);
        }
        catch (SecurityTokenException ex)
        {
            await WriteErrorAsync(context, HttpStatusCode.Unauthorized, Constant.JWT.InvalidAccessToken, ex.Message);
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, Constant.ErrorMessages.UnhandledException);
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, Constant.ErrorMessages.InternalServerError, ex.Message);
            return;
        }

        await _next(context);
    }

    #region === Private Helpers ===

    private static string? ExtractAccessToken(HttpContext context)
    {
        return context.Request.Headers[Constant.Authorization]
            .FirstOrDefault()?
            .Replace(Constant.JWT.Bearer, string.Empty);
    }

    private static string? ExtractRefreshToken(HttpContext context)
    {
        return context.Request.Headers[Constant.JWT.RefreshTokenHeader].FirstOrDefault();
    }

    private async Task HandleTokenValidationAsync(
        HttpContext context,
        IJWTService jwtService,
        string? accessToken,
        string? refreshToken)
    {
        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            await ValidateAccessTokenAsync(context, jwtService, accessToken);
        }
        else if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            await ValidateRefreshTokenAsync(context, jwtService, refreshToken);
        }
    }

    private async Task ValidateAccessTokenAsync(HttpContext context, IJWTService jwtService, string token)
    {
        ClaimsPrincipal principal = jwtService.GetPrincipalFromToken(token, validateLifetime: true);
        if (principal != null)
            context.User = principal;
    }

    private async Task ValidateRefreshTokenAsync(HttpContext context, IJWTService jwtService, string refreshToken)
    {
        bool isValid = jwtService.ValidateRefreshTokenAsync(refreshToken);

        if (!isValid)
        {
            await WriteErrorAsync(context, HttpStatusCode.Unauthorized, Constant.ErrorMessages.InvalidRefreshToken, Constant.JWT.InvalidRefreshToken);
            return;
        }

        string newAccessToken = await jwtService.GenerateAccessTokenFromRefreshTokenAsync(refreshToken);

        if (!string.IsNullOrEmpty(newAccessToken))
        {
            var principal = jwtService.GetPrincipalFromToken(newAccessToken, validateLifetime: true);
            if (principal != null)
            {
                context.User = principal;
                context.Response.Headers[Constant.Authorization] = $"{Constant.JWT.Bearer}{newAccessToken}";
            }
        }
    }

    private async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string error, string message)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = Constant.ContentTypes.ApplicationJson;

        var response = new { error, message };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    #endregion
}
