using Turnup.Entities;

namespace Turnup.Services.CartService;

public interface ICartService
{
    Task<ServiceResponse<Cart>> GetUserCart();

    Task<ServiceResponse<Cart>> CreateUserCart();

    Task<ServiceResponse<Cart>> AddItem();

    Task<ServiceResponse<Cart>> RemoveItem();
    
}