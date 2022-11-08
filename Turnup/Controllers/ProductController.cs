using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Entities;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly DbContext _context;

    public ProductController(DbContext context)
    {
        _context = context;
    }
    //[HttpGet]
    //public async Task<ActionResult<List<Product>>> GetProduct()
    // {
        // var products = await _context.Pr
        // return Ok(products)
    //}
}