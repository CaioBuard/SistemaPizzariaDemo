using System.ComponentModel.DataAnnotations;

namespace SistemaPizzariaDemo.ViewModels;

public class ConfiguracaoViewModel
{
    [Display(Name = "Limite máximo de entregas por motoboy")]
    [Range(1, 50, ErrorMessage = "Informe um limite entre 1 e 50.")]
    public int MaxEntregasPorMotoboy { get; set; } = 3;
}
