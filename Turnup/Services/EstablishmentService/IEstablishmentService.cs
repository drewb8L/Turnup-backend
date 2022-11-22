using Turnup.Entities;

namespace Turnup.Services.EstablishmentService;

public interface IEstablishmentService
{
    Task<ServiceResponse<Establishment>> CreateNewEstablishment(string name, string establishmentId);
    Task<ServiceResponse<Establishment>> DeleteEstablishment();

}