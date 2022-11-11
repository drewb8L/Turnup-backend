using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Turnup.Controllers;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class DevPlaygroundController : ControllerBase
{
    
    
    [HttpGet("playground")]
    public async Task<ActionResult<string>> Get()
    {
        return Ok("Hello World");
    }

    [HttpPost]
    [Route("post")]
    public async Task<ActionResult<string>> PostData(string text)
    {
        return Ok(text);
    }
}

