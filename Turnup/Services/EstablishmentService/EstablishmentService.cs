using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Turnup.Context;
using Turnup.Entities;

namespace Turnup.Services.EstablishmentService;

public class EstablishmentService : IEstablishmentService
{
    private readonly TurnupDbContext _context;
    private readonly IMapper _mapper;

    public EstablishmentService(TurnupDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<ServiceResponse<Establishment>> CreateNewEstablishment(string name, string establishmentId)
    {
        
        var newEstablishment = new ServiceResponse<Establishment>
        {
            Data = new Establishment()
            {
                Name = name,
                EstablishmentCode  = Guid.NewGuid().ToString(),
                Owner = establishmentId

            }
        };

       
        await _context.AddAsync(newEstablishment.Data);
        await _context.SaveChangesAsync();
        return newEstablishment;
    }

    public async Task<ServiceResponse<Establishment>> DeleteEstablishment()
    {
        throw new NotImplementedException();
    }
}