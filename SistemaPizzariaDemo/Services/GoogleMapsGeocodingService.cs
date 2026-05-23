using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using SistemaPizzariaDemo.Options;

namespace SistemaPizzariaDemo.Services;

public class GoogleMapsGeocodingService : IGoogleMapsGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly GoogleMapsOptions _options;
    private readonly ILogger<GoogleMapsGeocodingService> _logger;

    public GoogleMapsGeocodingService(
        HttpClient httpClient,
        IOptions<GoogleMapsOptions> options,
        ILogger<GoogleMapsGeocodingService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    public async Task<EnderecoGoogleResult> BuscarEnderecoAsync(
        string rua,
        string numero,
        CidadePermitida cidade,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            return EnderecoGoogleResult.Erro("Configure a chave do Google Maps no appsettings.json antes de buscar endereços.");
        }

        if (string.IsNullOrWhiteSpace(rua) || string.IsNullOrWhiteSpace(numero))
        {
            return EnderecoGoogleResult.Erro("Informe rua e número para consultar o Google Maps.");
        }

        var endereco = $"{rua}, {numero}, {cidade.Cidade} - {cidade.Uf}, {cidade.Pais}";
        var components = $"country:BR|administrative_area:{cidade.Uf}|locality:{cidade.Cidade}";
        var url = "https://maps.googleapis.com/maps/api/geocode/json"
            + $"?address={Uri.EscapeDataString(endereco)}"
            + $"&components={Uri.EscapeDataString(components)}"
            + $"&key={Uri.EscapeDataString(_options.ApiKey)}";

        try
        {
            using var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var googleResponse = await JsonSerializer.DeserializeAsync<GoogleGeocodeResponse>(
                stream,
                new JsonSerializerOptions(JsonSerializerDefaults.Web),
                cancellationToken);

            if (googleResponse is null)
            {
                return EnderecoGoogleResult.Erro("O Google Maps não retornou dados para esse endereço.");
            }

            if (!string.Equals(googleResponse.Status, "OK", StringComparison.OrdinalIgnoreCase))
            {
                return EnderecoGoogleResult.Erro($"Google Maps retornou status {googleResponse.Status}.");
            }

            var result = googleResponse.Results.FirstOrDefault();
            if (result is null)
            {
                return EnderecoGoogleResult.Erro("O Google Maps não encontrou esse endereço.");
            }

            var bairro = ObterComponente(result.AddressComponents, "sublocality_level_1")
                ?? ObterComponente(result.AddressComponents, "sublocality")
                ?? ObterComponente(result.AddressComponents, "neighborhood");

            if (string.IsNullOrWhiteSpace(bairro))
            {
                return EnderecoGoogleResult.Erro("O Google Maps encontrou o endereço, mas não retornou o bairro.");
            }

            var cidadeRetornada = ObterComponente(result.AddressComponents, "administrative_area_level_2")
                ?? ObterComponente(result.AddressComponents, "locality")
                ?? cidade.Cidade;
            var uf = ObterComponente(result.AddressComponents, "administrative_area_level_1", usarNomeCurto: true)
                ?? cidade.Uf;
            var pais = ObterComponente(result.AddressComponents, "country")
                ?? cidade.Pais;

            return new EnderecoGoogleResult
            {
                Sucesso = true,
                Bairro = bairro,
                Cidade = cidadeRetornada,
                Uf = uf,
                Pais = pais,
                EnderecoFormatado = result.FormattedAddress ?? endereco,
                Latitude = result.Geometry?.Location?.Lat,
                Longitude = result.Geometry?.Location?.Lng
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar endereço no Google Maps.");
            return EnderecoGoogleResult.Erro("Não foi possível consultar o Google Maps agora.");
        }
    }

    private static string? ObterComponente(
        IEnumerable<GoogleAddressComponent> componentes,
        string tipo,
        bool usarNomeCurto = false)
    {
        var componente = componentes.FirstOrDefault(c => c.Types.Contains(tipo));
        return usarNomeCurto ? componente?.ShortName : componente?.LongName;
    }

    private sealed class GoogleGeocodeResponse
    {
        public string Status { get; set; } = string.Empty;

        public List<GoogleGeocodeResult> Results { get; set; } = [];
    }

    private sealed class GoogleGeocodeResult
    {
        [JsonPropertyName("formatted_address")]
        public string? FormattedAddress { get; set; }

        [JsonPropertyName("address_components")]
        public List<GoogleAddressComponent> AddressComponents { get; set; } = [];

        public GoogleGeometry? Geometry { get; set; }
    }

    private sealed class GoogleAddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; } = string.Empty;

        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = string.Empty;

        public List<string> Types { get; set; } = [];
    }

    private sealed class GoogleGeometry
    {
        public GoogleLocation? Location { get; set; }
    }

    private sealed class GoogleLocation
    {
        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}
