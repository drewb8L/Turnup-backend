using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Turnup.Controllers;
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "customer")]
//[Authorize(Roles = "customer")]
[Route("api/[controller]")]
[ApiController]
public class DevPlaygroundController : ControllerBase
{
    
    
    [HttpGet]
    public async Task<ActionResult<string>> Get()
    {
        var headers = HttpContext.Request.Headers.Authorization.ToString();
        return Ok($" Hello! " +
                  $"Headers: {headers}");
    }

    [HttpPost]
    [Route("post")]
    public async Task<ActionResult<string>> PostData(string text)
    {
        return Ok(text);
    }
    
    
}

