using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;

namespace Turnup.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly TurnupDbContext _context;

    public OrderController(TurnupDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetOrder")]
    public async Task<ActionResult<OrderDTO>> Get(string customerId, string establishmentId)
    {
        
        var order = _context.Orders
              .Where(o => o.CustomerId == customerId && o.EstablishmentId == establishmentId)
              .FirstOrDefaultAsync();

        if (order is null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [HttpGet]
    [Route("retrieve-order")]
    public async Task<ActionResult<Order>> RetrieveOrder(string establishmentId)
    {
        var customerId = User.Claims.FirstOrDefault().Value;
        var orders = await _context.Orders
            .Where(c => c.CustomerId == customerId && c.EstablishmentId == establishmentId)
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Product)
            .ToListAsync();
        
        
        return Ok(orders);
    }

    [HttpGet]
    [Route("place-order")]
    public async Task<ActionResult<Order>> PlaceOrder()
    {
        var customerId = User.Claims.FirstOrDefault().Value;
        var cart = await _context.Carts.Where(c => c.CustomerId == customerId)
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync();
        
        
        var order = new Order
        {
            CustomerId = customerId,
            EstablishmentId = cart.EstablishmentId,
            OrderDate = DateTime.Now,
            OrderItems = cart.Items,
            Status  = OrderStatus.Pending.ToString(),
            SubTotal = cart.Subtotal,
        };

        order.Total = order.CalculateTotal();

        await _context.Orders.AddAsync(order);
        //_context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
        
        
        return Ok(order);

    }

    
}