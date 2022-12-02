using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.CartService;

namespace Turnup.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly TurnupDbContext _context;
    private string _establishmentId;
    private readonly ICartService _cartService;
    private Claim? _user;

    public CartController(TurnupDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    [HttpGet(Name = "GetCart")]
    public async Task<ActionResult<CartDTO>> Get(string establishmentId)
    {
        _establishmentId = establishmentId;
        _user = User.Claims.FirstOrDefault();
        var cart = await _cartService.GetUserCart(establishmentId, _user);
        if (cart.Data is null) return new CartDTO();

        return MapCartToDto(cart.Data);
    }

    [HttpPost("add-items")]
    public async Task<ActionResult<CartDTO>> AddItemToCart(int productId, int quantity)
    {
        _user = User.Claims.FirstOrDefault();
        var cart = await _cartService.AddItem(productId, quantity, _user); // ?? CreateCart();

        var product = await _context.Products.FindAsync(productId);
        if (product is null) return NotFound();

        if (cart.Data == null)
            return BadRequest(new ProblemDetails { Title = "There's an issue saving your item to the cart!" });

        var result = await SaveNewProduct(quantity, cart, product);
        if (result) return CreatedAtRoute("GetCart", MapCartToDto(cart.Data));

        return BadRequest(new ProblemDetails { Title = "There's an issue saving your item to the cart!" });
    }

    private async Task<bool> SaveNewProduct(int quantity, ServiceResponse<Cart> cart, Product product)
    {
        cart.Data.AddItem(product, quantity);
        cart.Data.Subtotal = cart.Data.CalculateSubtotal();
        var result = await _context.SaveChangesAsync() > 0;
        return result;
    }

    [HttpDelete]
    public async Task<ActionResult> RemoveCartItem(int productId, int quantity)
    {
        
        var cart = await GetCart(productId);
        if (cart is null) return NotFound();
        var product = await _context.Products.FindAsync(productId);
        if (product is null) return NotFound();
        cart.RemoveItem(product.Id, quantity);
        cart.Subtotal = cart.SubtractSubTotal();
        if (cart.Items.Count == 0)
        {
            cart.Subtotal = 0.0m;
        }

        var result = await _context.SaveChangesAsync() > 0;
        if (result) return StatusCode(201);

        return BadRequest(new ProblemDetails { Title = "There's an issue removing your item to the cart!" });
    }


    private async Task<Cart?> GetCart(int productId)
    {
        var product = await _context.Products.FindAsync(productId);
        if (product is null)
        {
            return new Cart();
        }
        var user = User.Claims.FirstOrDefault();
        var cart= await _cartService.GetUserCart(product.EstablishmentId, user);
        return cart.Data;
    }
    

    private CartDTO MapCartToDto(Cart cart)
    {
        return new CartDTO
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId, //User.Claims.FirstOrDefault().Value,
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