namespace Turnup.Entities.OrderEntities;

public class OrderItem
{
    public int Id { get; set; }
    public ProductOrdered ProductOrdered { get; set; }
    public long Price { get; set; }
    public int Quantity { get; set; }
}