namespace Turnup.Entities;

public class Cart
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public List<CartItem> Items { get; set; } = new();

    public string EstablishmentId { get; set; } = string.Empty;

    public decimal Subtotal { get; set; }
    public void AddItem(Product product, int quantity)
    {
        if (Items.All(item => item.ProductId != product.Id))
        {
            Items.Add(new CartItem { Product = product, Quantity = quantity });
            EstablishmentId = product.EstablishmentId;
        }

        var existingItem = Items.FirstOrDefault(item => item.ProductId == product.Id);
            if (existingItem != null) existingItem.Quantity += quantity;
            
       
            
    }
    public void RemoveItem(int productId, int quantity)
    {
        var item = Items.FirstOrDefault(item => item.ProductId == productId);
        if (item == null) return;
        item.Quantity -= quantity;
        if (item.Quantity <= 0) Items.Remove(item);
       
    }

    public decimal CalculateSubtotal()
    {
        var total = 0.0m;
         foreach (var cartItem in Items)
        {
            total += cartItem.Quantity * cartItem.Product.Price;
        }

        return total;
    }

    public decimal SubtractSubTotal()
    {
        var total = Subtotal;
        foreach (var cartItem in Items)
        {
            if (Items.Count <= 0)
            {
                return 0.0m;
            }
            total -= cartItem.Quantity * cartItem.Product.Price;
        }

        return total;
    }
}