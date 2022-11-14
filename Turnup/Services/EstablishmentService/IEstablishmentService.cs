using Turnup.Entities;

namespace Turnup.Services.EstablishmentService;

public interface IEstablishmentService
{
    Task<ServiceResponse<Establishment>> CreateNewEstablishment(string name);
    Task<ServiceResponse<Establishment>> DeleteEstablishment();

}