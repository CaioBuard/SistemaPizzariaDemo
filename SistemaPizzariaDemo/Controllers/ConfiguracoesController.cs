using Microsoft.AspNetCore.Mvc;
using SistemaPizzariaDemo.Services;
using SistemaPizzariaDemo.ViewModels;

namespace SistemaPizzariaDemo.Controllers;

public class ConfiguracoesController : Controller
{
    private readonly IConfiguracaoService _configuracaoService;

    public ConfiguracoesController(IConfiguracaoService configuracaoService)
    {
        _configuracaoService = configuracaoService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var limite = await _configuracaoService
            .ObterLimiteMaximoEntregasPorMotoboyAsync(cancellationToken);

        return View(new ConfiguracaoViewModel
        {
            MaxEntregasPorMotoboy = limite
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ConfiguracaoViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        await _configuracaoService
            .SalvarLimiteMaximoEntregasPorMotoboyAsync(model.MaxEntregasPorMotoboy, cancellationToken);

        TempData["Mensagem"] = "Configurações salvas.";
        return RedirectToAction(nameof(Index));
    }
}
