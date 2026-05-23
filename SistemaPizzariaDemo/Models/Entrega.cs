using System.ComponentModel.DataAnnotations;

namespace SistemaPizzariaDemo.Models;

public class Entrega
{
    public int Id { get; set; }

    [Display(Name = "Número do pedido")]
    [Required(ErrorMessage = "Informe o número do pedido.")]
    [StringLength(30, ErrorMessage = "O número do pedido deve ter no máximo 30 caracteres.")]
    public string NumeroPedido { get; set; } = string.Empty;

    [Display(Name = "Quantidade de pizzas")]
    [Range(1, 200, ErrorMessage = "Informe uma quantidade válida de pizzas.")]
    public int QuantidadePizzas { get; set; } = 1;

    [Display(Name = "Rua pesquisada")]
    [Required(ErrorMessage = "Informe a rua ou termo de busca.")]
    [StringLength(200, ErrorMessage = "A rua deve ter no máximo 200 caracteres.")]
    public string RuaPesquisada { get; set; } = string.Empty;

    [Display(Name = "Número")]
    [Required(ErrorMessage = "Informe o número.")]
    [StringLength(20, ErrorMessage = "O número deve ter no máximo 20 caracteres.")]
    public string Numero { get; set; } = string.Empty;

    [Display(Name = "Complemento")]
    [StringLength(120, ErrorMessage = "O complemento deve ter no máximo 120 caracteres.")]
    public string? Complemento { get; set; }

    [Required]
    [StringLength(120)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    [StringLength(120)]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string Uf { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string Pais { get; set; } = "Brasil";

    [Required]
    [StringLength(500)]
    public string EnderecoFormatado { get; set; } = string.Empty;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public EntregaStatus Status { get; set; } = EntregaStatus.AguardandoSaida;

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public DateTime? SaiuParaEntregaEm { get; set; }

    public DateTime? FinalizadaEm { get; set; }
}
