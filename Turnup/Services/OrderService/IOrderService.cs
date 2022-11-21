using Turnup.Entities.OrderEntities;

namespace Turnup.Services.OrderService;

public interface IOrderService
{
    Task<ServiceResponse<Order>> GetOrder(string establishmentId);

    Task<ServiceResponse<Order>> PlaceOrder();
    
}