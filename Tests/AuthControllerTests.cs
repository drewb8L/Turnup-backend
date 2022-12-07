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
        var user = _fixture.Create<AuthUser>();
        var newUser = new AuthUserRegisterDTO()
        {
            Name = "Test123",
            Email = "test123@aol.com",
            Password = "MyPass@word1"
        };

        var newAuthUser = new AuthUser()
        {
            Name = newUser.Name,
            Email = newUser.Email,
            Role = Roles.customer.ToString()
        };

        var service = await _authService.CreateNewUser(newAuthUser, newUser.Password);
        //_controller = new AuthenticationController();
        //var result = await _controller.Register(newUser, Roles.customer);

        Assert.True(service.Succeeded);

    }
    
    

}