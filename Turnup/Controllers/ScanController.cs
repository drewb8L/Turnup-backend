using Microsoft.AspNetCore.Mvc;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.ScanService;

namespace Turnup.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ScanController : ControllerBase
{


    private readonly IScanService _scanService;
    public ScanController(IScanService scanService)
    {
        _scanService = scanService;
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<List<Product>>>> GetEstablishment(string establishmentCode)
    {
        try
        {
            var products = await _scanService.GetEstablishmentProducts(establishmentCode);
            return Ok(products.Data);
        }
        catch (Exception)
        {
            
            return NotFound();
        }
        
    }
}