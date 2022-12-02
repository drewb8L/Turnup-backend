using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Turnup.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Turnup.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<AuthUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<AuthUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }
    
    public string GenerateJwtToken(AuthUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Role, user.Role ),
                new Claim("Name", user.Name),
                new Claim("Role",user.Role),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Email, value: user.Email),
                new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }

    public async Task<AuthUser?> FindByEmail(string email)
    {
       return await _userManager.FindByEmailAsync(email);
    }

    public  async Task<IdentityResult> RegisterNewUser(AuthUser newUser, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> CreateNewUser(AuthUser newUser, string password)
    {
      return await _userManager.CreateAsync(newUser, password);
    }

    public async Task<bool> CheckPasswordAsync(AuthUser? existingUser, string password)
    {
        return await _userManager.CheckPasswordAsync(existingUser, password);
    }
}