namespace Turnup.Entities.OrderEntities;

public class Order
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public List<OrderItem> OrderItems { get; set; }
    public long SubTotal { get; set; }
    public long ServiceFee { get; set; }
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

    public long GetTotal()
    {
        return SubTotal + ServiceFee;
    }
    
    

}