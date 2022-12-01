using Turnup.Entities;

namespace Turnup.Services.ScanService;

public interface IScanService
{
    Task<ServiceResponse<List<Product>>> GetEstablishmentProducts(string establishmentCode);
}