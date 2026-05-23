namespace SistemaPizzariaDemo.Services;

public class EnderecoGoogleResult
{
    public bool Sucesso { get; init; }

    public string? Mensagem { get; init; }

    public string Bairro { get; init; } = string.Empty;

    public string Cidade { get; init; } = string.Empty;

    public string Uf { get; init; } = string.Empty;

    public string Pais { get; init; } = "Brasil";

    public string EnderecoFormatado { get; init; } = string.Empty;

    public double? Latitude { get; init; }

    public double? Longitude { get; init; }

    public static EnderecoGoogleResult Erro(string mensagem)
    {
        return new EnderecoGoogleResult
        {
            Sucesso = false,
            Mensagem = mensagem
        };
    }
}
