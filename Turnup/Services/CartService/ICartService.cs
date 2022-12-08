using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Turnup.Entities;

namespace Turnup.Services.CartService;

public interface ICartService
{
    Task<ServiceResponse<Cart>> GetUserCart(string establishmentId, Claim? user);

   

    Task<ServiceResponse<Cart>> AddItem(int productId, int quantity, Claim? user);
    Task<bool> SaveNewProduct(int quantity, ServiceResponse<Cart> cart, Product product);
    Task<bool> RemoveCartItem(int productId, int quantity, Cart cart, Product product);
    Task<ServiceResponse<Cart>> GetCart(Claim? user);
    
    
}