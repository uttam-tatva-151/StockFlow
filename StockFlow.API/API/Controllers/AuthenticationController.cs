using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockFlow.API.Controllers;
using StockFlow.Common.Models;
using StockFlow.Service.DTOs.User;
using StockFlow.Service.DTOs.UserAuth;
using StockFlow.Service.Interfaces;

namespace StockFlow.API;

public class AuthenticationController(IUserAuthService UserAuthService) : BaseController
{
    private readonly IUserAuthService _UserAuthService = UserAuthService;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] UserAuthDTO request, CancellationToken cancellationToken)
    {
        ApiResponse response = await _UserAuthService.AuthenticateUserAsync(request, cancellationToken);

        return Ok(response);
    }
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] NewUserDTO request, CancellationToken cancellationToken)
    {
        ApiResponse response = await _UserAuthService.RegisterUserAsync(request, cancellationToken);
        return Ok(response);
    }
}
