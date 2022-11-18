namespace Turnup.Entities.OrderEntities;

public class OrderItem
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public string EstablishmentId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}