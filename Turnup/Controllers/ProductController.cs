using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Services;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly TurnupDbContext _context;

    public ProductController(TurnupDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProduct()
    {
        var products = await _context.Products.ToListAsync();
        var response = new ServiceResponse<List<Product>>()
        {
            Data = products
        };
        return Ok(response);
    }
}