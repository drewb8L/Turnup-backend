using Turnup.Entities.OrderEntities;

namespace Turnup.DTOs;

public class OrderDTO
{
    public string CustomerId { get; set; }
    public string EstablishmentId { get; set; }
    public static List<OrderItem> Items { get; set; }

    //public DateTime OrderDate { get; set; }
    public static decimal SubTotal { get; set; } = GetSubTotal();
    public static decimal Tax { get; set; } = 0.635m;
    public decimal Total { get; set; } = CalculateTotal();
    
    public static decimal GetSubTotal()
    {
        var subtotal = 0.0m;
        foreach (var product in Items)
        {
            subtotal += product.Quantity + product.Price;
        }

        return subtotal;
    }

    public static decimal CalculateTotal()
    {
        return (SubTotal * Tax) + SubTotal;
    }

    
}

public class OrderItemDTO
{
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal SubTotal { get; set; }

    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    
    
}