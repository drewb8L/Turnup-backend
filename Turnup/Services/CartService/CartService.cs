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
        var response = new ServiceResponse<Cart?>
        {
             Data = await _context.Carts.Where(c => 
                 c.CustomerId == _user.Subject.Claims.FirstOrDefault().Value 
                 && c.EstablishmentId == establishmentId).FirstOrDefaultAsync()
            
        };

        return response;
    }
    private async Task<ServiceResponse<Cart>> CreateUserCart()
    {
        var customerId = _user.Subject.Claims.FirstOrDefault().Value;
        var response = new ServiceResponse<Cart>
        {
            Data =  new Cart { CustomerId = customerId }

        };
        await _context.Carts.AddAsync(response.Data);
        await _context.SaveChangesAsync();
        return response;
    }


   

    public async Task<ServiceResponse<Cart>> AddItem()
    {
        var response = await GetUserCart(_establishmentId, _user);
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
    
    private async Task<Cart?> GetCart()
    {
        return await _context.Carts
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == _user.Subject.Claims.FirstOrDefault().Value);
        
    }

    private  Cart CreateCart()
    {
        var customerId =_user.Subject.Claims.FirstOrDefault().Value;
        var cart = new Cart { CustomerId = customerId};
        _context.Carts.Add(cart);
        return cart;
    }
    
    private CartDTO MapCartToDto(Cart cart)
    {
        
        return new CartDTO
        {
            Id = cart.Id,
            CustomerId = _user.Subject.Claims.FirstOrDefault().Value,
            EstablishmentId = _establishmentId,
            Items = cart.Items.Select(item => new CartItemDTO
            {
                ProductId = item.ProductId,
                ImgUrl = item.Product.ImageUrl,
                Name = item.Product.Title,
                Price = item.Product.Price,
                Quantity = item.Quantity,
               
            }).ToList(),
            Subtotal = cart.CalculateSubtotal()
        };
    }
   
}