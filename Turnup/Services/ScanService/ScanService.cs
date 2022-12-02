using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Services.ProductService;

namespace Turnup.Services.ScanService;

public class ScanService : IScanService
{
    private readonly TurnupDbContext _context;
    private readonly IProductService _productService;
    public ScanService(TurnupDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
        
    }
    
    public async Task<ServiceResponse<List<Product>>> GetEstablishmentProducts(string establishmentCode)
    {
        var establishment = await _context.Establishments
            .Where(e => e.EstablishmentCode == establishmentCode).FirstAsync();

        var products = await _productService.GetProductsAsync(establishment.Owner);
        
        return products;
    }
}