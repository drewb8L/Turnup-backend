using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
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

    [HttpGet]
    [Route("retrieve-order")]
    public async Task<ActionResult<Order>> RetrieveOrder(string customerId)
    {
        var items = await _context.Carts.Where(c => c.CustomerId == customerId)
            .Include(i => i.Items)
            
            .ToListAsync();
        return Ok(items);
    }

    [HttpPost]
    [Route("place-order")]
    public async Task<ActionResult<Order>> PlaceOrder(string customerId)
    {
        var items = await _context.Carts.Where(c => c.CustomerId == customerId).ToListAsync();
        return Ok(items);

    }
}