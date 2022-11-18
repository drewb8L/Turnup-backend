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
    
    public decimal SubTotal { get; set; }
    //public long ServiceFee { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal Total { get; set; }

    public void GetTotal()
    {
        var subtotal = 0.0m;
        foreach (var product in OrderItems)
        {
            subtotal += product.Quantity + product.Product.Price;
        }

        SubTotal = subtotal;
    }
    
    

}