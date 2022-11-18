using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turnup.DTOs;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;
using Turnup.Services;
using Turnup.Services.ProductService;

namespace Turnup.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly IProductService _productService;
    public MenuController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet]
    [Route("get-establishment-menu")]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts(string establishmentId)
    {
        var result = await _productService.GetProductsAsync(establishmentId);
        return Ok(result);
    }

    [HttpPost]
    [Route("place-order")]
    public async Task<ActionResult<ServiceResponse<Order>>> PlaceOrder()
    {
        throw new NotImplementedException();
    }
    
}