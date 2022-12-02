using System.Security.Claims;
using Turnup.Entities;

namespace Turnup.Services.CartService;

public interface ICartService
{
    Task<ServiceResponse<Cart>> GetUserCart(string establishmentId, Claim? user);

   

    Task<ServiceResponse<Cart>> AddItem(int productId, int quantity, Claim? user);

    Task<ServiceResponse<Cart>> RemoveItem();
    Task<ServiceResponse<Cart>> GetCart(Claim? user);
    
    
}