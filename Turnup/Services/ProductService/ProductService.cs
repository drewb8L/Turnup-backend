using Microsoft.AspNetCore.Mvc;
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
    public async Task<ServiceResponse<List<Product>>> GetProductsAsync(string establishmentId)
    {
        
        var response = new ServiceResponse<List<Product>>
        {
            Data = await _context.Products.Where(p => p.EstablishmentId == establishmentId).ToListAsync()
        };

        return response;
    }

    public async Task<ServiceResponse<Product>> CreateNewProduct(string title, string description, string imageUrl, decimal price, string establishmentId )
    {
        var newProduct = new ServiceResponse<Product>
        {
            Data = new Product()
            {
                Title = title,
                Description = description,
                ImageUrl = imageUrl,
                Price = price,
                EstablishmentId = establishmentId
            }

        };

         await _context.AddAsync(newProduct.Data);
        await _context.SaveChangesAsync();

        return newProduct;
    }
}