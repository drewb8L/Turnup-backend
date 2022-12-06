using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;
using Turnup.Services.CartService;

namespace Turnup.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly TurnupDbContext _context;
    private readonly Claim? _user;
    private ICartService _cartService;

    public OrderService(TurnupDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }


    public async Task<ServiceResponse<List<Order>>> GetOrder(string establishmentId, Claim? user)
    {
        var customerId = user.Subject.Claims.FirstOrDefault().Value;
        

        var response = new ServiceResponse<List<Order>>()
        {
            Data = await _context.Orders
                .Where(c => c.CustomerId == customerId && c.EstablishmentId == establishmentId)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product).ToListAsync()

        };

        return response;

    }

    public async Task<ServiceResponse<Cart>> PlaceOrder(Claim? user)
    {
      return await _cartService.GetCart(user);
    }
}