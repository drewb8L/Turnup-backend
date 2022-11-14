using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Turnup.Entities;
using Turnup.Services;
using Turnup.Services.EstablishmentService;

namespace Turnup.Controllers;

// Authorize for admin only
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IEstablishmentService _establishmentService;
    private readonly IMapper _mapper;

    public AdminController(IEstablishmentService establishmentService, IMapper mapper)
    {
        _establishmentService = establishmentService;
        _mapper = mapper;
    }
    
    
    [HttpPost]
    public async Task<ActionResult<ServiceResponse<EstablishmentDTO>>> CreateEstablishment(string name)
    {
        
        var result = await _establishmentService.CreateNewEstablishment(name);
        return Ok(result);
    }
    

    
}