using Authentication.Services.Interfaces;
using Authentication.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var result = await _identityService.LoginAsync(loginRequest);
        if (result != null) return Ok(result);
        return BadRequest(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginRequest loginRequest)
    {
        var result = await _identityService.RegisterAsync(loginRequest);

        return StatusCode(result.Succeded ? StatusCodes.Status200OK : StatusCodes.Status400BadRequest, result);
    }

    [HttpPost("validate")]
    [Authorize]
    public async Task<IActionResult> ValidateToken()
    {
        return await Task.FromResult(Ok("Token is valid"));
    }


    [HttpPost("logout")]
    public async Task<IActionResult> SignOu()
    {
        await _identityService.SignOut();
        return Ok("Successfully logged out");
    }
}