using System.Security.Claims;
using Turnup.Entities.OrderEntities;

namespace Turnup.Services.OrderService;

public interface IOrderService
{
    Task<ServiceResponse<List<Order>>> GetOrder(string establishmentId, Claim? user);

    Task<ServiceResponse<Order>> PlaceOrder();
    
}