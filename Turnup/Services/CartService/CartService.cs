using System.Security.Claims;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;

namespace Turnup.Services.CartService;

public class CartService : ICartService
{
    private readonly TurnupDbContext _context;

    private string _establishmentId;
    private Claim? _user;

    public CartService(TurnupDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<Cart>> GetUserCart(string establishmentId, Claim? user)
    {
        _establishmentId = establishmentId;
        _user = user;
        var response = new ServiceResponse<Cart>
        {
            Data = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .Where(c =>
                    c.CustomerId == _user.Subject.Claims.FirstOrDefault().Value
                    && c.EstablishmentId == establishmentId).FirstOrDefaultAsync()
        };
        return response;
    }

    public async Task<ServiceResponse<Cart>> AddItem(int productId, int quantity, Claim? user)
    {
        var record = await GetCart(user);
        if (record.Data is null)
        {
            var cart = CreateCart(user);
            var response = new ServiceResponse<Cart>()
            {
                Data = cart
            };

            return response;
        }
        return record;
        
    }

    public Task<ServiceResponse<Cart>> RemoveItem()
    {
        throw new NotImplementedException();
    }

    
    public async Task<ServiceResponse<Cart>> GetCart(Claim? user)
    {
        _user = user;
        var cart = await _context.Carts
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == _user.Subject.Claims.FirstOrDefault().Value);

        var response = new ServiceResponse<Cart>()
        {
            Data = cart
        };
        return response;
    }

    private Cart CreateCart(Claim? user)
    {
        _user = user;
        var customerId = _user.Subject.Claims.FirstOrDefault().Value;
        var cart = new Cart { CustomerId = customerId };
        _context.Carts.Add(cart);
        return cart;
    }
}