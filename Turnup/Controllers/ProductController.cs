using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.ProductService;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        
        _productService = productService;
    }
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
    {
        var result = await _productService.GetProductsAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceResponse<Product>>> CreateNewProduct(string title, string description, string imageUrl, long price )
    {
        var result = await _productService.CreateNewProduct(title, description, imageUrl, price);
        return Ok(result);
    }
}