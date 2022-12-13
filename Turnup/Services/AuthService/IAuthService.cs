using Microsoft.AspNetCore.Identity;
using Turnup.Entities;

namespace Turnup.Services.AuthService;

public interface IAuthService
{
    public string GenerateJwtToken(AuthUser user);

    public Task<AuthUser?> FindByEmail(string email);

    public Task<IdentityResult> CreateNewUser(AuthUser user, string password);

    public Task<bool> CheckPasswordAsync(AuthUser? existingUser, string password);
}