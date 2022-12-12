using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NuGet.Protocol;
using Turnup.Controllers;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.AuthService;


namespace Tests;

public class AuthControllerTests
{
    //private Mock<IAuthService> _authService;
    private IAuthService _authService;
    private Fixture _fixture;
    private AuthenticationController _controller;
    private UserManager<AuthUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthControllerTests()
    {
        _fixture = new Fixture();
        
        _authService = new AuthService(_userManager, _configuration);
    }

    [Fact]
    public async Task Register()
    {
        

    }
    
    

}