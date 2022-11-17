using System.ComponentModel.DataAnnotations;

namespace Turnup.Entities.OrderEntities;

public class Order
{   
    [Key]
    [MaxLength(256)]
    public string CustomerId { get; set; }
    [MaxLength(256)]
    public string EstablishmentId { get; set; }
    
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    public List<Product> OrderItems { get; set; }
    public long SubTotal { get; set; }
    public long ServiceFee { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    public long GetTotal()
    {
        return SubTotal + ServiceFee;
    }
    
    

}