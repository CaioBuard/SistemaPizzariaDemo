using SistemaPizzariaDemo.Models;

namespace SistemaPizzariaDemo.ViewModels;

public class PainelEntregasViewModel
{
    public int LimitePorMotoboy { get; set; }

    public int TotalEntregasPendentes { get; set; }

    public int TotalMotoboysDisponiveis { get; set; }

    public IReadOnlyList<GrupoEntregasViewModel> Grupos { get; set; } = [];
}

public class GrupoEntregasViewModel
{
    public int Ordem { get; set; }

    public int? MotoboyId { get; set; }

    public string MotoboyNome { get; set; } = string.Empty;

    public bool SemMotoboy { get; set; }

    public string Bairro { get; set; } = string.Empty;

    public string Cidade { get; set; } = string.Empty;

    public string Uf { get; set; } = string.Empty;

    public string CorClasse { get; set; } = string.Empty;

    public IReadOnlyList<Entrega> Entregas { get; set; } = [];
}
