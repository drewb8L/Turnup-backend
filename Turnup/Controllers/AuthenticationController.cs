using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Services.AuthService;


namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    
    private readonly IAuthService _authService;
    
    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }


    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult> Register([FromBody] AuthUserRegisterDTO requestDto, Roles role)
    {
        if (ModelState.IsValid)
        {
            var userExists = await _authService.FindByEmail(requestDto.Email);

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

            
            var newUser = new AuthUser
            {
                Name = requestDto.Name,
                UserName = requestDto.Email,
                Email = requestDto.Email,
                Role = role.ToString()
            };

            var createUser = await _authService.CreateNewUser(newUser, requestDto.Password);
            if (createUser.Succeeded)
            {
                var token = _authService.GenerateJwtToken(newUser);
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
    public async Task<ActionResult> Login([FromBody] AuthUserLoginDTO authUserLoginDto)
    {
        if (ModelState.IsValid)
        {
            var existingUser = await _authService.FindByEmail(authUserLoginDto.Email);
            if (existingUser is null)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid information"
                    },
                    Result = false
                });

            var validInfo = await _authService.CheckPasswordAsync(existingUser, authUserLoginDto.Password);

            if (!validInfo)
                return BadRequest(new AuthResult()
                {
                    Errors = new List<string>()
                    {
                        "Invalid Credentials"
                    },
                    Result = false
                });

            var jwtToken = _authService.GenerateJwtToken(existingUser);
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

    
}