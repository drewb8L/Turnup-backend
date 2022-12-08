using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Turnup.Context;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.CartService;
using Turnup.Services.ProductService;

namespace Turnup.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]
[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    
    private string? _establishmentId;
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private Claim? _user;

    public CartController( ICartService cartService, IProductService productService)
    {
        
        _cartService = cartService;
        _productService = productService;
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

        //var product = await _context.Products.FindAsync(productId);
        var product = await _productService.GetProductAsync(productId);
        if (product.Data is null) return NotFound();

        if (cart.Data == null)
            return BadRequest(new ProblemDetails { Title = "There's an issue saving your item to the cart!" });

        var result = await _cartService.SaveNewProduct(quantity, cart, product.Data);
        if (result) return CreatedAtRoute("GetCart", MapCartToDto(cart.Data));

        return BadRequest(new ProblemDetails { Title = "There's an issue saving your item to the cart!" });
    }
    

    [HttpDelete]
    public async Task<ActionResult> RemoveCartItem(int productId, int quantity)
    {
        var user = User.Claims.FirstOrDefault();
        var cart = await _cartService.GetCart(user);
        if (cart.Data is null) return NotFound();

        var product = await _productService.GetProductAsync(productId);
        if (product.Data is null) return NotFound();
        
        
        var result = await _cartService.RemoveCartItem(productId, quantity, cart.Data, product.Data);
        if (result) return StatusCode(201);

        return BadRequest(new ProblemDetails { Title = "There's an issue removing your item to the cart!" });
    }

    
    private CartDTO MapCartToDto(Cart cart)
    {
        return new CartDTO
        {
            Id = cart.Id,
            CustomerId = cart.CustomerId,
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