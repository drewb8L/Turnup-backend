namespace Turnup.DTOs;

public class CartDTO
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public List<CartItemDTO> Items { get; set; }
}


// TODO: Move to file
public class CartItemDTO
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string ImgUrl { get; set; }
    public int Quantity { get; set; }
    
}