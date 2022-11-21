using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.OpenApi.Extensions;

namespace Turnup.Entities.OrderEntities;

public class Order
{   
    [Key] 
    public int OrderId { get; set; }
    [MaxLength(256)]
    public string CustomerId { get; set; }
    [MaxLength(256)]
    public string EstablishmentId { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    public List<OrderItem> OrderItems { get; set; }
    public  decimal Tax { get; set; } = 0.0635m;
    public  decimal SubTotal { get; set; }
    //public long ServiceFee { get; set; }
    public string Status { get; set; } 

    public decimal Total { get; set; } 

    public void GetSubTotal()
    {
        var subtotal = 0.0m;
        foreach (var product in OrderItems)
        {
            subtotal += product.Quantity + product.Product.Price;
        }

        SubTotal = subtotal;
    }

    public  decimal CalculateTotal()
    {
        return (SubTotal * Tax) + SubTotal;
    }
    

}