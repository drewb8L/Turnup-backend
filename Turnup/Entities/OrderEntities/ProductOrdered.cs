using Microsoft.EntityFrameworkCore;

namespace Turnup.Entities.OrderEntities;

[Owned]
public class ProductOrdered
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
}