using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;

namespace Turnup.Services.CartService;

public class CartService : ICartService
{
    private readonly TurnupDbContext _context;

    public CartService(TurnupDbContext context)
    {
        _context = context;
    }
    
    public Task<ServiceResponse<Cart>> GetUserCart(string customerId)
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<Cart>> CreateUserCart()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<Cart>> AddItem()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponse<Cart>> RemoveItem()
    {
        throw new NotImplementedException();
    }
    
   
}