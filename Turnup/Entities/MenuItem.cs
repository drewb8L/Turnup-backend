namespace Turnup.Entities;

public class MenuItem
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int MenuId { get; set; }
    public Menu Menu { get; set; }
}