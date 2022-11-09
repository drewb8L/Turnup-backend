using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly TurnupDbContext _context;

    public CartController(TurnupDbContext context)
    {
        _context = context;
    }

    [HttpGet(Name = "GetCart")]
    public async Task<ActionResult<CartDTO>> Get()
    {
        // TODO: Find storage for CustomerId outside of cookies
        var cart = await GetCart();

        if (cart is null) return new CartDTO(); //NotFound();

        return MapCartToDto(cart);

    }

    


    [HttpPost("add-items")]
    public async Task<ActionResult<CartDTO>> AddItemToCart(int productId, int quantity)
    {
        var cart = await GetCart() ?? CreateCart();
        var product = await _context.Products.FindAsync(productId);
        if (product is null) return NotFound();
        cart.AddItem(product, quantity);
        var result = await _context.SaveChangesAsync() > 0;
        if(result) return CreatedAtRoute("GetCart", MapCartToDto(cart));
        Console.WriteLine($"Result:{result}");
        Console.WriteLine($"Cookie{Request.Headers.Cookie.ToString()}");
        
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
        Console.WriteLine($"Result:{result}");
        Console.WriteLine($"Cookie{Request.Headers.Cookie.ToString()}");
        
        return BadRequest(new ProblemDetails { Title = "There's an issue removing your item to the cart!" });
    }
    
    
    private async Task<Cart?> GetCart()
    {
        
        return await _context.Carts
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(c => c.CustomerId == Request.Cookies["CustomerId"]);
        
        
    }

    private Cart CreateCart()
    {
        var customerId = Guid.NewGuid().ToString();
        var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
        Response.Cookies.Append("customerId", customerId, cookieOptions);
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

/*
 * Request.Headers.Cookie.ToString()
 * Store customerID curl gets new id everytime
 * Options for replacing cookies
 * Write to file
 * Maui Preferences
 * Maui storage
*/