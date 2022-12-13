using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;

namespace Turnup.Services.CartService;

public class CartService : ICartService
{
    private readonly TurnupDbContext _context;

    
    private Claim? _user;

    public CartService(TurnupDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<Cart>> GetUserCart(string establishmentId, Claim? user)
    {
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

    public async Task<bool> RemoveCartItem(int productId, int quantity, Cart cart, Product product)
    {
        cart.RemoveItem(product.Id, quantity);
        cart.Subtotal = cart.SubtractSubTotal();
        if (cart.Items.Count == 0)
        {
            cart.Subtotal = 0.0m;
        }

        return await _context.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> SaveNewProduct(int quantity, ServiceResponse<Cart> cart, Product product)
    {
        cart.Data.AddItem(product, quantity);
        cart.Data.Subtotal = cart.Data.CalculateSubtotal();
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }
    
    
    private CartDTO MapCartToDto(Cart cart)
    {
        return new CartDTO
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            EstablishmentId = cart.EstablishmentId,
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