using System.Collections;
using System.Security.Claims;
using AutoFixture;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Turnup.Context;
using Turnup.Controllers;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;
using Turnup.Services;
using Turnup.Services.CartService;
using Turnup.Services.OrderService;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Text.Json;
using NuGet.Protocol;

namespace Tests;

public class OrderControllerTests
{
    private Mock<IOrderService> _orderService;
    private Mock<ICartService> _cartService;
    
    private Fixture _fixture;
    private OrderController _controller;

    public OrderControllerTests()
    {
        _fixture = new Fixture();
        _orderService = new Mock<IOrderService>();
        _cartService = new Mock<ICartService>();
        
    }

    [Fact]
    [Authorize]
    public async Task RetrieveOrderByEstablishmentId()
    {
        var establishment = _fixture.Create<Establishment>();
        var user = _fixture.Create<AuthUser>();
        var sr = new ServiceResponse<List<Order>>()
        {
            Data = new List<Order>()
            {
                new Order()
                {
                    CustomerId = user.Id,
                    EstablishmentId = establishment.Id.ToString(),
                    OrderDate = DateTime.Now,

                }
            }
        };
        
        var claimsPrincipal = Helpers.CustomerClaimsPrincipal(user, out var claims, out var identity);

        _orderService.Setup(o => 
            o.GetOrder(establishment.Id.ToString(), claimsPrincipal.Claims.FirstOrDefault()))
            .ReturnsAsync(sr);
        
        _controller = new OrderController(_orderService.Object);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = claimsPrincipal
            }
        };

        var result = await _controller.RetrieveOrder(establishment.Id.ToString());

        var obj = result.Result as OkObjectResult;
        var jsonObj = JsonSerializer.Deserialize<List<Order>>(obj.Value.ToJson());
        
        Assert.NotNull(jsonObj);
        Assert.Contains("0", jsonObj.FirstOrDefault().OrderId.ToString());
    }

    [Fact]
    [Authorize]
    public async Task PlaceOrderByCustomerId()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        var order = _fixture.Create<Order>();
        var user = _fixture.Create<AuthUser>();
        var claimsPrincipal = Helpers.CustomerClaimsPrincipal(user, out var claims, out var identity);
        var claim = claimsPrincipal.Claims.FirstOrDefault();
        order.CustomerId = claim.Value;
        
        var sr = new ServiceResponse<Order>()
        {
            Data = order
            
        };
        

        _orderService.Setup(o => o.PlaceOrder(claim)).ReturnsAsync(sr);
        
        _controller = new OrderController(_orderService.Object);
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
            {
                User = claimsPrincipal
            }
        };

        var result = await _controller.PlaceOrder();

        var obj = result.Result as CreatedAtActionResult;

        var jsonObj = JsonSerializer.Deserialize<Order>(obj.Value.ToJson());

        Assert.NotNull(jsonObj);
        Assert.Equal("0", jsonObj.OrderId.ToString());
       
    }
}

