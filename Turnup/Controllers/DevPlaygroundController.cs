using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DevPlaygroundController : ControllerBase
{
    [HttpGet("playground")]
    public async Task<ActionResult<string>> Get()
    {
        return Ok("Hello World");
    }
}