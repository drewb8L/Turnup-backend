using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;
using Turnup.Services.OrderService;

namespace Turnup.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]
[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly TurnupDbContext _context;
    private readonly IOrderService _orderService;
    public OrderController(TurnupDbContext context, IOrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

  

    [HttpGet]
    [Route("retrieve-order")]
    public async Task<ActionResult<Order>> RetrieveOrder(string establishmentId)
    {
        var customerId = User.Claims.FirstOrDefault();
        var orders = await _orderService.GetOrder(establishmentId, customerId);
        
        return Ok(orders.Data);
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
            OrderItems = new List<OrderItem>(),
            Status  = OrderStatus.Pending.ToString(),
            SubTotal = cart.Subtotal,
        };

        order.OrderItems.AddRange(MapCartItemToOrderItem(cart.Items));
        order.Total = order.CalculateTotal();

        await _context.Orders.AddAsync(order);
        _context.Carts.Remove(cart);
        await _context.SaveChangesAsync();
        
        
        return CreatedAtAction("RetrieveOrder", order);

    }

    private List<OrderItem> MapCartItemToOrderItem(List<CartItem> cartItems)
    {
        List<OrderItem> items = new List<OrderItem>();
        
        foreach (var item in cartItems)
        {
            var orderItem = new OrderItem
            {
                EstablishmentId = item.Product.EstablishmentId,
                CustomerId = item.Cart.CustomerId,
                Product = item.Product,
                Title = item.Product.Title,
                Quantity = item.Quantity,
                Price = item.Product.Price,
                ProductId = item.ProductId

            };

            items.Add(orderItem);
        }

        return items;
    }


}