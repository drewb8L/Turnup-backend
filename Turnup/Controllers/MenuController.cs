using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    [HttpGet]
    [Route("menu")]
    public async Task<ActionResult> GetEstablishmentMenu(int id)
    {
        return Ok();
    }

    [HttpPost]
    [Route("add-products")]
    public async Task<ActionResult> AddProductToMene()
    {
        return Ok();
    }
}