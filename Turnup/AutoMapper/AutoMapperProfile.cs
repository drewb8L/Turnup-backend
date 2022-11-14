using AutoMapper;
using Turnup.Entities;
using Turnup.Services.EstablishmentService;

namespace Turnup.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Establishment, EstablishmentDTO>();
    }
}