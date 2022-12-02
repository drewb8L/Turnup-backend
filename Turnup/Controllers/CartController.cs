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
    private readonly TurnupDbContext _context;
    private string? _establishmentId;
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private Claim? _user;

    public CartController(TurnupDbContext context, ICartService cartService, IProductService productService)
    {
        _context = context;
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
        var user = User.Claims.FirstOrDefault();
        var cart = await _cartService.GetCart(user);
        if (cart.Data is null) return NotFound();

        var product = await _productService.GetProductAsync(productId);
        if (product.Data is null) return NotFound();
        
        
        cart.Data.RemoveItem(product.Data.Id, quantity);
        cart.Data.Subtotal = cart.Data.SubtractSubTotal();
        if (cart.Data.Items.Count == 0)
        {
            cart.Data.Subtotal = 0.0m;
        }

        var result = await _context.SaveChangesAsync() > 0;
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