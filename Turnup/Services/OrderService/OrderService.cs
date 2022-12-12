using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Turnup.Context;
using Turnup.Entities;
using Turnup.Entities.OrderEntities;
using Turnup.Services.CartService;

namespace Turnup.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly TurnupDbContext _context;
    private readonly Claim? _user;
    private ICartService _cartService;

    public OrderService(TurnupDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }


    public async Task<ServiceResponse<List<Order>>> GetOrder(string establishmentId, Claim? user)
    {
        var customerId = user.Subject.Claims.FirstOrDefault().Value;
        

        var response = new ServiceResponse<List<Order>>()
        {
            Data = await _context.Orders
                .Where(c => c.CustomerId == customerId && c.EstablishmentId == establishmentId)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.Product).ToListAsync()

        };

        return response;

    }

    public async Task<ServiceResponse<Order>> PlaceOrder(Claim? user)
    {
      var cart= await _cartService.GetCart(user);
      var order = Order(cart);

      return new ServiceResponse<Order>()
      {
          Data = order.Result
      };
    }
    
    private async Task<Order> Order(ServiceResponse<Cart> cart)
    {
        var order = new Order
        {
            CustomerId = cart.Data.CustomerId,
            EstablishmentId = cart.Data.EstablishmentId,
            OrderDate = DateTime.Now,
            OrderItems = new List<OrderItem>(),
            Status = OrderStatus.Pending.ToString(),
            SubTotal = cart.Data.Subtotal,
        };

        order.OrderItems.AddRange(MapCartItemToOrderItem(cart.Data.Items));
        order.Total = order.CalculateTotal();

        await _context.Orders.AddAsync(order);
        _context.Carts.Remove(cart.Data);
        await _context.SaveChangesAsync();
        return order;
    }
    
    private List<OrderItem> MapCartItemToOrderItem(List<CartItem> cartItems)
    {
        List<OrderItem> items = new List<OrderItem>();
        
        foreach (var item in cartItems)
        {
            var orderItem = new OrderItem
            {
                EstablishmentId = item.Product.EstablishmentId,
                CustomerId = item.Cart.CustomerId,
                Product = item.Product,
                Title = item.Product.Title,
                Quantity = item.Quantity,
                Price = item.Product.Price,
                ProductId = item.ProductId

            };

            items.Add(orderItem);
        }

        return items;
    }
}