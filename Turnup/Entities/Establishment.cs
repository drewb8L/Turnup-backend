using System.ComponentModel.DataAnnotations;

namespace Turnup.Entities;

public class Establishment
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; }

    // public Menu Menu { get; set; } = new();
    // public int MenuId { get; set; }

    public List<Product> Products { get; set; } = new();
}