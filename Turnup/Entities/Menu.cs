namespace Turnup.Entities;

public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MenuItem> MenuItems { get; set; }

    public int EstablishmentId { get; set; }
    
    public void AddItem(Product product)
    {
        if (MenuItems.All(item => item.ProductId != product.Id))
        {
            MenuItems.Add(new MenuItem { Product = product});
        }

        // var existingItem = MenuItems.FirstOrDefault(item => item.ProductId == product.Id);
        // if (existingItem != null) existingItem.Quantity += quantity;
    }
    public void RemoveItem(int productId)
    {
        var item = MenuItems.FirstOrDefault(item => item.ProductId == productId);
        if (item == null) return;
        // item.Quantity -= quantity;
        // if (item.Quantity <= 0) Items.Remove(item);
    }
    
}