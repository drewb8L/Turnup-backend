using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Turnup.Entities.OrderEntities;

public class Order
{   
    [Key]
    [MaxLength(256)]
    public string CustomerId { get; set; }
    [MaxLength(256)]
    public string EstablishmentId { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    public List<CartItem> OrderItems { get; set; }
    public static decimal Tax { get; set; } = 0.0635m;
    public static decimal SubTotal { get; set; }
    //public long ServiceFee { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    public decimal Total { get; set; } = CalculateTotal();

    public void GetTotal()
    {
        var subtotal = 0.0m;
        foreach (var product in OrderItems)
        {
            subtotal += product.Quantity + product.Product.Price;
        }

        SubTotal = subtotal;
    }

    public static decimal CalculateTotal()
    {
        return (SubTotal * Tax) + SubTotal;
    }
    

}