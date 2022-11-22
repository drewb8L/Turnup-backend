using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;
using Turnup.Services;
using Turnup.Services.EstablishmentService;
using Turnup.Services.ProductService;

namespace Turnup.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "establishment, admin" )]

[Route("api/[controller]")]
[ApiController]
public class EstablishmentController : ControllerBase
{
    
    private readonly IProductService _productService;
    private readonly TurnupDbContext _context;
    private readonly IEstablishmentService _establishmentService;

    public EstablishmentController(TurnupDbContext context, IProductService productService, IEstablishmentService establishmentService)
    {
        
        _productService = productService;
        _context = context;
        _establishmentService = establishmentService;

    }
    
    [HttpGet]
    [Route("products")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
    {
        var userId = User.Claims.FirstOrDefault().Value;
        var result = await _productService.GetProductsAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("add-product")]
    public async Task<ActionResult<ServiceResponse<Product>>> CreateNewProduct(string title, string description, string imageUrl, long price )
    {
        var userId = User.Claims.FirstOrDefault().Value;
        var result = await _productService.CreateNewProduct(title, description, imageUrl, price, userId);
        return Ok(result);
    }

    [HttpGet]
    [Route("pending-orders")]
    public async Task<ActionResult<Order>> GetOrders()
    {
        var pendingOrders = await _context.Orders
            .Where(o => o.EstablishmentId == User.Claims.FirstOrDefault().Value && o.Status == OrderStatus.Pending.ToString())
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Product)
            .OrderBy(o =>o.OrderDate)
            .ToListAsync();

        return Ok(pendingOrders);
    }

    [HttpPatch]
    [Route("update-order")]
    public async Task<ActionResult<Order>> UpdateOrCompleteOrder(int orderId, OrderStatus status)
    {
        var order = await _context.Orders.Where(o => o.OrderId == orderId)
            .Include(o => o.OrderItems)
            .ThenInclude(i => i.Product).FirstOrDefaultAsync();

        order.Status = status.ToString();

        _context.Orders.Update(order);
        _context.SaveChangesAsync();

        return Ok(order);
    }

    [HttpPost]
    [Route("create-establishment-link")]
    public async Task<ActionResult<ServiceResponse<Establishment>>> CreatEstablishment(string name)
    {
        var establishmentId = User.Claims.FirstOrDefault().Value;
        var result = _establishmentService.CreateNewEstablishment(name, establishmentId);
        

        
        
        return Ok(result.Result.Data);
    }

   
}