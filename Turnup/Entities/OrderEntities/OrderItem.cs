using System.ComponentModel.DataAnnotations;

namespace Turnup.Entities.OrderEntities;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }
    
    public string CustomerId { get; set; }
    
    public string EstablishmentId { get; set; }
    
    public string Title { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}