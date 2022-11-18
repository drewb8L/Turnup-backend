using Turnup.Entities.OrderEntities;

namespace Turnup.DTOs;

public class OrderDTO
{
    public string CustomerId { get; set; }
    public string EstablishmentId { get; set; }
    public List<OrderItem> Items { get; set; }
    
    
}

public class OrderItemDTO
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }

    public void calculateSubTotal()
    {
        SubTotal = Quantity * Price;
    }
    
}