using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Services.ProductService;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScanController : ControllerBase
{
    private readonly TurnupDbContext _context;
    private readonly IProductService _productService;


    public ScanController(TurnupDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;

    }

    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetEstablishment(string establishmentCode)
    {
        var establishment = await _context.Establishments
            .Where(e => e.EstablishmentCode == establishmentCode)
            .FirstOrDefaultAsync();

        var products = await _productService.GetProductsAsync(establishment.Owner);
            
        return Ok(products);
    }
}