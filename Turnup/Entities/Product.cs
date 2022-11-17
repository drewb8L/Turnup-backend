using System.ComponentModel.DataAnnotations;

namespace Turnup.Entities;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public long Price { get; set; }

    [MaxLength(256)]
    public string EstablishmentId { get; set; }
    
}