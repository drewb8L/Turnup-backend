using System.Security.Claims;
using Turnup.Entities;

namespace Tests;

public class Helpers
{
    private static ClaimsPrincipal ClaimsPrincipal(Establishment establishment, out List<Claim> claims,
        out ClaimsIdentity identity)
    {
        claims = new List<Claim>()
        {
            new Claim("Id", establishment.Id.ToString()),
            new Claim(ClaimTypes.Role, Roles.establishment.ToString()),
        };
        identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return claimsPrincipal;
    }
}