using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("register")]
    public IActionResult Register(RegisterRequest loginRequest)
    {
        return Ok();
    }
}