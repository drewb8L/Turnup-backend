using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.ProductService;

namespace Turnup.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "establishment")]
[Route("api/[controller]")]
[ApiController]
public class EstablishmentController : ControllerBase
{
    
    private readonly IProductService _productService;

    public EstablishmentController(TurnupDbContext context, IProductService productService)
    {
        
        _productService = productService;
        
    }
    
    [HttpGet]
    [Route("products")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
    {
        var userId = User.Claims.FirstOrDefault().Value;
        var result = await _productService.GetProductsAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route("add-product")]
    public async Task<ActionResult<ServiceResponse<Product>>> CreateNewProduct(string title, string description, string imageUrl, long price )
    {
        var userId = User.Claims.FirstOrDefault().Value;
        var result = await _productService.CreateNewProduct(title, description, imageUrl, price, userId);
        return Ok(result);
    }
    
}