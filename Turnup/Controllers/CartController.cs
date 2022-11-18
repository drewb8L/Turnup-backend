using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Services.CartService;

namespace Turnup.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;
    
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet(Name = "GetCart")]
    public async Task<ActionResult<CartDTO>> Get()
    {
        var customerId = User.Claims.FirstOrDefault().Value;
        var cart = await _cartService.GetUserCart(customerId);

        if (cart.Data is null) return new CartDTO(); //NotFound();

        Console.WriteLine($"JWT: {User.Claims.FirstOrDefault().Value}");
        return MapCartToDto(cart.Data);

    }

    


    [HttpPost("add-items")]
    public async Task<ActionResult<CartDTO>> AddItemToCart(int productId, int quantity)
    {
        var cart = _cartService.GetUserCart(User.Claims.FirstOrDefault().Value);
                  
        var product = await _context.Products.FindAsync(productId);
        if (product is null) return NotFound();
        cart.AddItem(product, quantity);
        var result = await _context.SaveChangesAsync() > 0;
        if(result) return CreatedAtRoute("GetCart", MapCartToDto(cart));
        
        
        return BadRequest(new ProblemDetails { Title = "There's an issue saving your item to the cart!" });
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveCartItem(int productId, int quantity)
    {
        var cart = await GetCart();
        if (cart is null) return NotFound();
        var product = await _context.Products.FindAsync(productId);
        if (product is null) return NotFound();
        cart.RemoveItem(product.Id, quantity);
        var result = await _context.SaveChangesAsync() > 0;
        if(result) return StatusCode(201);
        
        
        return BadRequest(new ProblemDetails { Title = "There's an issue removing your item to the cart!" });
    }
    
    
    private async Task<Cart?> GetCart()
    {
        
        return await _context.Carts
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == User.Claims.FirstOrDefault().Value );
        
        
    }

    private Cart CreateCart()
    {
        var customerId = User.Claims.FirstOrDefault().Value;
        var cart = new Cart { CustomerId = customerId };
        _context.Carts.Add(cart);
        return cart;
    }
    
    private CartDTO MapCartToDto(Cart cart)
    {
        return new CartDTO
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
            Items = cart.Items.Select(item => new CartItemDTO
            {
                ProductId = item.ProductId,
                ImgUrl = item.Product.ImageUrl,
                Name = item.Product.Title,
                Price = item.Product.Price,
                Quantity = item.Quantity
            }).ToList()
        };
    }
}

