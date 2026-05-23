using System.ComponentModel.DataAnnotations;
using SistemaPizzariaDemo.Options;

namespace SistemaPizzariaDemo.ViewModels;

public class EntregaCreateViewModel
{
    [Display(Name = "Número do pedido")]
    [Required(ErrorMessage = "Informe o número do pedido.")]
    [StringLength(30)]
    public string NumeroPedido { get; set; } = string.Empty;

    [Display(Name = "Quantidade de pizzas")]
    [Range(1, 200, ErrorMessage = "Informe uma quantidade válida de pizzas.")]
    public int QuantidadePizzas { get; set; } = 1;

    [Display(Name = "Cidade")]
    [Required(ErrorMessage = "Selecione a cidade.")]
    public string CidadeSelecionada { get; set; } = string.Empty;

    [Display(Name = "Rua")]
    [Required(ErrorMessage = "Informe a rua.")]
    [StringLength(200)]
    public string RuaPesquisada { get; set; } = string.Empty;

    [Display(Name = "Número")]
    [Required(ErrorMessage = "Informe o número.")]
    [StringLength(20)]
    public string Numero { get; set; } = string.Empty;

    [Display(Name = "Complemento")]
    [StringLength(120)]
    public string? Complemento { get; set; }

    [Required(ErrorMessage = "Busque o endereço no Google Maps antes de salvar.")]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    public string Uf { get; set; } = string.Empty;

    [Required]
    public string Pais { get; set; } = "Brasil";

    [Required]
    public string EnderecoFormatado { get; set; } = string.Empty;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public IReadOnlyList<CidadePermitida> CidadesPermitidas { get; set; } = [];
}
