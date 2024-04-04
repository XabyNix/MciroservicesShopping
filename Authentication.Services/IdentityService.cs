using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Data.Models;
using Authentication.Services.Interfaces;
using Authentication.Shared.Models;
using Authentication.Shared.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Services;

public class IdentityService : IIdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public IdentityService(IOptions<JwtSettings> jwtSettings, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
    {
        var signInResult = await _signInManager
            .PasswordSignInAsync(loginRequest.Username, loginRequest.Password, false, false);
        if (!signInResult.Succeeded) return null;

        //var user = await _userManager.FindByNameAsync(loginRequest.Username);
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecurityKey));
        var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, loginRequest.Username),
            new Claim(ClaimTypes.Email, loginRequest.Email),
            new Claim(ClaimTypes.Role, "Customer")
        };

        var securityToken = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(1),
            signInCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        var result = new AuthResponse { AuthenticationToken = tokenString };
        return await Task.FromResult(result);
    }


    public async Task<RegisterResponse> RegisterAsync(LoginRequest loginRequest)
    {
        var user = new ApplicationUser
        {
            Email = loginRequest.Email,
            UserName = loginRequest.Username
        };
        var result = await _userManager.CreateAsync(user, loginRequest.Password);

        var response = new RegisterResponse
        {
            Succeded = result.Succeeded,
            Errors = result.Errors.Select(e => e.Description)
        };

        return response;
    }

    public async Task SignOut()
    {
        await _signInManager.SignOutAsync();
    }
}