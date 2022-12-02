using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;

namespace Turnup.Services.CartService;

public class CartService : ICartService
{
    private readonly TurnupDbContext _context;
    private readonly ClaimsPrincipal _user;
    public string UserId { get; set; }

    public CartService(TurnupDbContext context, ClaimsPrincipal user)
    {
        _context = context;
        _user = user;
        
    }
    
    public async Task<ServiceResponse<Cart>> GetUserCart()
    {
        var response = new ServiceResponse<Cart?>
        {
             Data = await _context.Carts.Where(c => c.CustomerId == _user.Claims.FirstOrDefault().Value).FirstOrDefaultAsync()
            
        };

        return response;
    }
    private async Task<ServiceResponse<Cart>> CreateUserCart()
    {
        var customerId = _user.Claims.FirstOrDefault().Value;
        var response = new ServiceResponse<Cart>
        {
            Data =  new Cart { CustomerId = customerId }

        };
        await _context.Carts.AddAsync(response.Data);
        await _context.SaveChangesAsync();
        return response;
    }


    public Task<ServiceResponse<Cart>> GetUserCart(string customerId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Cart>> AddItem()
    {
        var response = await GetUserCart();
        if (response.Data is null)
        {
            return await CreateUserCart();
            
        }

        return response;
    }

    public Task<ServiceResponse<Cart>> RemoveItem()
    {
        throw new NotImplementedException();
    }
    
   
}