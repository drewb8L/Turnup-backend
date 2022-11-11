using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Turnup.Configurations;
using Turnup.DTOs;
using Turnup.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IConfiguration _configuration;
    //private readonly JwtConfig _jwtConfig;

    public AuthenticationController(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
        //_jwtConfig = jwtConfig;
    }


    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromBody] UserRegistrationRequestDTO requestDto)
    {
        if (ModelState.IsValid)
        {
            var userExists = await _userManager.FindByEmailAsync(requestDto.Email);

            if (userExists is not null)
            {
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email already exists"
                    }
                });
            }

            var newUser = new IdentityUser()
            {
                UserName = requestDto.Email,
                Email = requestDto.Email,
            };

            var createUser = await _userManager.CreateAsync(newUser, requestDto.Password);
            if (createUser.Succeeded)
            {
                var token = GenerateJwtToken(newUser);
                return Ok(new AuthResult()
                {
                    Result = true,
                    Token = token
                });
            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                {
                    "Server error",
                },
                Result = false
            });
        }

        return BadRequest();
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody] LogingRequestDTO logingRequestDto)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _userManager.FindByEmailAsync(logingRequestDto.Email);
            if (existingUser is null)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid information"
                    },
                    Result = false
                });

            var validInfo = await _userManager.CheckPasswordAsync(existingUser, logingRequestDto.Password);

            if (!validInfo)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid Credentials"
                    },
                    Result = false
                });

            var jwtToken = GenerateJwtToken(existingUser);
            return Ok(new AuthResult()
            {
                Token = jwtToken,
                Result = true

            });
        }

        return BadRequest(new AuthResult()
        {
            Errors = new List<string>()
            {
                "Invalid payload"
            },
            Result = false,
        });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSection("JwtConfig:Secret").Value);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", user.Id),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, value: user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };
        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var jwtToken = jwtTokenHandler.WriteToken(token);

        return jwtToken;
    }
}