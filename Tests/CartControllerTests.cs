using System.Text.Json;
using AutoFixture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NuGet.Protocol;
using Turnup.Controllers;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.CartService;
using Turnup.Services.ProductService;



namespace Tests;

public class CartControllerTests
{
    private Mock<ICartService> _cartService;
    private Mock<IProductService> _productService;
    private CartController _controller;
    private Fixture _fixture;

    public CartControllerTests()
    {
        _fixture = new Fixture();
        _cartService = new Mock<ICartService>();
        _productService = new Mock<IProductService>();
    }


    [Fact]
    [Authorize]
    public async Task GetANewCartOfNullValues()
    {
        FixtureBehaviors();

        var establishment = _fixture.Create<Establishment>();
        var cart = _fixture.Create<Cart>();
        var customer = _fixture.Create<AuthUser>();
        var claimsPrincipal = Helpers.CustomerClaimsPrincipal(customer, out var claims, out var identity);
        var claim = claimsPrincipal.Claims.FirstOrDefault();
        var sr = new ServiceResponse<Cart>()
        {
            Data = new Cart()
           
        };
        cart.CustomerId = claim.Value;
        cart.EstablishmentId = establishment.Id.ToString();

        _cartService.Setup(c => c.GetCart(claim)).ReturnsAsync(sr);
        
        _controller = new CartController(_cartService.Object, _productService.Object);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = claimsPrincipal
            }
        };

        var result = await _controller.Get(establishment.Id.ToString());
        var obj = result.Result as ObjectResult;
        
        
        Assert.Null(obj);
    }

    private void FixtureBehaviors()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
}