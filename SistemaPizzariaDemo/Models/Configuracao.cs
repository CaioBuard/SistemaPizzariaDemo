using System.ComponentModel.DataAnnotations;

namespace SistemaPizzariaDemo.Models;

public class Configuracao
{
    [Key]
    [StringLength(80)]
    public string Chave { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Valor { get; set; } = string.Empty;
}
