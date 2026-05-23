using SistemaPizzariaDemo.Options;

namespace SistemaPizzariaDemo.Services;

public interface IGoogleMapsGeocodingService
{
    Task<EnderecoGoogleResult> BuscarEnderecoAsync(
        string rua,
        string numero,
        CidadePermitida cidade,
        CancellationToken cancellationToken = default);
}
