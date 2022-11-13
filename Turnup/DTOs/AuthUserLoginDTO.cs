using System.ComponentModel.DataAnnotations;

namespace Turnup.DTOs;

public class AuthUserLoginDTO
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}