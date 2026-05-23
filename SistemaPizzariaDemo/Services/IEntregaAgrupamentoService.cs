using SistemaPizzariaDemo.Models;
using SistemaPizzariaDemo.ViewModels;

namespace SistemaPizzariaDemo.Services;

public interface IEntregaAgrupamentoService
{
    PainelEntregasViewModel MontarPainel(
        IReadOnlyList<Entrega> entregas,
        IReadOnlyList<Motoboy> motoboys,
        int limitePorMotoboy);
}
