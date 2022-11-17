using Turnup.Entities;

namespace Turnup.Services.ProductService;

public interface IProductService
{
    Task<ServiceResponse<List<Product>>> GetProductsAsync(string establishmentId);

    Task<ServiceResponse<Product>> CreateNewProduct(string title, string description, string imageUrl, long price, string establishmentId);
}