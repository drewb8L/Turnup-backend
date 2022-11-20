namespace Turnup.Entities;

public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<MenuItem> MenuItems { get; set; }

    public int EstablishmentId { get; set; }
    
    
}