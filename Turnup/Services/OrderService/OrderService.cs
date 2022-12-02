using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities.OrderEntities;

namespace Turnup.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly TurnupDbContext _context;
    private readonly Claim? _user;

    public OrderService(TurnupDbContext context)
    {
        _context = context;
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

    public async Task<ServiceResponse<Order>> PlaceOrder()
    {
        throw new NotImplementedException();
    }
}