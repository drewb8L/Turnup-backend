using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities.OrderEntities;

namespace Turnup.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly TurnupDbContext _context;
    private readonly ClaimsPrincipal _user;

    public OrderService(TurnupDbContext context, ClaimsPrincipal user)
    {
        _context = context;
        _user = user;
    }


    public async Task<ServiceResponse<Order>> GetOrder(string establishmentId)
    {
        var customerId = _user.Claims.FirstOrDefault().Value;
        var cartItems = await _context.Carts.Where(c => c.CustomerId == customerId && c.EstablishmentId == establishmentId)
            .Include(i => i.Items)
            .Include(c => c.Subtotal)
            .ToListAsync();

        throw new NotImplementedException();

    }

    public async Task<ServiceResponse<Order>> PlaceOrder()
    {
        throw new NotImplementedException();
    }
}