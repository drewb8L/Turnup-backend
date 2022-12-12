using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;
using Turnup.Services;
using Turnup.Services.CartService;
using Turnup.Services.OrderService;

namespace Turnup.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    
    private readonly IOrderService _orderService;
    
    public OrderController( IOrderService orderService)
    {
        
        _orderService = orderService;
        
    }

  

    [HttpGet]
    [Route("retrieve-order")]
    public async Task<ActionResult<List<Order>>> RetrieveOrder(string establishmentId)
    {
        var customerId = User.Claims.FirstOrDefault();
        var orders = await _orderService.GetOrder(establishmentId, customerId);
        
        return Ok(orders.Data);
    }

    [HttpGet]
    [Route("place-order")]
    public async Task<ActionResult<Order>> PlaceOrder()
    {
        var customerId = User.Claims.FirstOrDefault();
        var order = await _orderService.PlaceOrder(customerId);

       return CreatedAtAction("RetrieveOrder", order);
    }

    

}