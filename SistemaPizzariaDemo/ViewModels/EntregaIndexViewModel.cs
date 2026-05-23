using SistemaPizzariaDemo.Models;

namespace SistemaPizzariaDemo.ViewModels;

public class EntregaIndexViewModel
{
    public EntregaCreateViewModel NovaEntrega { get; set; } = new();

    public IReadOnlyList<Entrega> Entregas { get; set; } = [];
}
