using Turnup.Entities;

namespace Turnup.DTOs;

public class UserDTO
{
    public string Username { get; set; }
    public string Password { get; set; }

    public UserType UserType { get; set; }
}