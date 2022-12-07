using System.Security.Claims;
using Turnup.Entities;

namespace Tests;

public static class Helpers
{
    public static ClaimsPrincipal EstablishmentClaimsPrincipal(Establishment establishment, out List<Claim> claims,
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
    
    public static ClaimsPrincipal CustomerClaimsPrincipal(AuthUser user, out List<Claim> claims,
        out ClaimsIdentity identity)
    {
        claims = new List<Claim>()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, Roles.customer.ToString()),
        };
        identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        return claimsPrincipal;
    }
}