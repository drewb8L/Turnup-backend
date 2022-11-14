using Microsoft.AspNetCore.Identity;

namespace Turnup.Entities;

public class AuthUser : IdentityUser
{
    public string Name { get; set; }
    public string Role { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}