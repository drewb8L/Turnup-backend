using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;

namespace Turnup.Services.ProductService;

public class ProductService : IProductService
{
    private readonly TurnupDbContext _context;

    public ProductService(TurnupDbContext context)
    {
        _context = context;
    }
    public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
    {
        
        var response = new ServiceResponse<List<Product>>
        {
            Data = await _context.Products.ToListAsync()
        };

        return response;
    }
}