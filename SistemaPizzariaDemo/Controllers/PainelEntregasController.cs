using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPizzariaDemo.Data;
using SistemaPizzariaDemo.Models;
using SistemaPizzariaDemo.Services;

namespace SistemaPizzariaDemo.Controllers;

public class PainelEntregasController : Controller
{
    private readonly PizzariaDbContext _context;
    private readonly IConfiguracaoService _configuracaoService;
    private readonly IEntregaAgrupamentoService _agrupamentoService;

    public PainelEntregasController(
        PizzariaDbContext context,
        IConfiguracaoService configuracaoService,
        IEntregaAgrupamentoService agrupamentoService)
    {
        _context = context;
        _configuracaoService = configuracaoService;
        _agrupamentoService = agrupamentoService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var entregas = await _context.Entregas
            .AsNoTracking()
            .Where(e => e.Status == EntregaStatus.AguardandoSaida)
            .ToListAsync(cancellationToken);

        var motoboys = await _context.Motoboys
            .AsNoTracking()
            .Where(m => m.Ativo && m.Disponivel)
            .ToListAsync(cancellationToken);

        var limite = await _configuracaoService
            .ObterLimiteMaximoEntregasPorMotoboyAsync(cancellationToken);

        var painel = _agrupamentoService.MontarPainel(entregas, motoboys, limite);

        return View(painel);
    }
}
