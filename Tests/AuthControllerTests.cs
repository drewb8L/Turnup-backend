using AutoFixture;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Turnup.Controllers;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.AuthService;


namespace Tests;

public class AuthControllerTests
{
    private Mock<IAuthService> _authService;
    private Fixture _fixture;
    private AuthenticationController _controller;

    public AuthControllerTests()
    {
        _fixture = new Fixture();
        _authService = new Mock<IAuthService>();
    }

    [Fact]
    public async Task Register()
    {
        var user = _fixture.Create<AuthUser>();

        _authService.Setup(r => r.FindByEmail(user.Email)).ReturnsAsync(new AuthUser());
        _controller = new AuthenticationController(_authService.Object);
        
        
        
    }

}