using Authentication.Shared.Models;

namespace Authentication.Services.Interfaces;

public interface IIdentityService
{
    public Task<RegisterResponse> RegisterAsync(LoginRequest loginRequest);
    public Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
    public Task SignOut();
}