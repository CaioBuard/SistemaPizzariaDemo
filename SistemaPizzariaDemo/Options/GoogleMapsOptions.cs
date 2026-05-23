namespace SistemaPizzariaDemo.Options;

public class GoogleMapsOptions
{
    public string ApiKey { get; set; } = string.Empty;

    public List<CidadePermitida> AllowedCities { get; set; } = [];
}

public class CidadePermitida
{
    public string Cidade { get; set; } = string.Empty;

    public string Uf { get; set; } = string.Empty;

    public string Pais { get; set; } = "Brasil";

    public string Valor => $"{Cidade}|{Uf}|{Pais}";

    public string Texto => $"{Cidade} - {Uf}";
}
