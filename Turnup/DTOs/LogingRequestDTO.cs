using System.ComponentModel.DataAnnotations;

namespace Turnup.DTOs;

public class LogingRequestDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}