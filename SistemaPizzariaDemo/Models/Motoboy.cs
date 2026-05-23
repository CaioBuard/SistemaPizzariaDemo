using System.ComponentModel.DataAnnotations;

namespace SistemaPizzariaDemo.Models;

public class Motoboy
{
    public int Id { get; set; }

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "Informe o nome do motoboy.")]
    [StringLength(120, ErrorMessage = "O nome deve ter no máximo 120 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Telefone")]
    [StringLength(30, ErrorMessage = "O telefone deve ter no máximo 30 caracteres.")]
    public string? Telefone { get; set; }

    [Display(Name = "Placa")]
    [StringLength(15, ErrorMessage = "A placa deve ter no máximo 15 caracteres.")]
    public string? Placa { get; set; }

    public bool Ativo { get; set; } = true;

    [Display(Name = "Disponível")]
    public bool Disponivel { get; set; } = true;

    public DateTime CriadoEm { get; set; } = DateTime.Now;

    public DateTime? AtualizadoEm { get; set; }

    public DateTime? InativadoEm { get; set; }
}
