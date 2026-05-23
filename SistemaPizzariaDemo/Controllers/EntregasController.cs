using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SistemaPizzariaDemo.Data;
using SistemaPizzariaDemo.Models;
using SistemaPizzariaDemo.Options;
using SistemaPizzariaDemo.Services;
using SistemaPizzariaDemo.ViewModels;

namespace SistemaPizzariaDemo.Controllers;

public class EntregasController : Controller
{
    private readonly PizzariaDbContext _context;
    private readonly IGoogleMapsGeocodingService _googleMaps;
    private readonly GoogleMapsOptions _googleMapsOptions;

    public EntregasController(
        PizzariaDbContext context,
        IGoogleMapsGeocodingService googleMaps,
        IOptions<GoogleMapsOptions> googleMapsOptions)
    {
        _context = context;
        _googleMaps = googleMaps;
        _googleMapsOptions = googleMapsOptions.Value;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await CriarIndexViewModelAsync(new EntregaCreateViewModel(), cancellationToken));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastrar(
        [Bind(Prefix = "NovaEntrega")] EntregaCreateViewModel model,
        CancellationToken cancellationToken)
    {
        var cidadePermitida = ObterCidadePermitida(model.CidadeSelecionada);
        if (cidadePermitida is null)
        {
            ModelState.AddModelError("NovaEntrega.CidadeSelecionada", "Selecione uma cidade válida.");
        }

        if (!ModelState.IsValid)
        {
            return View("Index", await CriarIndexViewModelAsync(model, cancellationToken));
        }

        var entrega = new Entrega
        {
            NumeroPedido = model.NumeroPedido.Trim(),
            QuantidadePizzas = model.QuantidadePizzas,
            RuaPesquisada = model.RuaPesquisada.Trim(),
            Numero = model.Numero.Trim(),
            Complemento = model.Complemento,
            Bairro = model.Bairro,
            Cidade = model.Cidade,
            Uf = model.Uf,
            Pais = model.Pais,
            EnderecoFormatado = model.EnderecoFormatado,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            Status = EntregaStatus.AguardandoSaida,
            CriadoEm = DateTime.Now
        };

        _context.Entregas.Add(entrega);
        await _context.SaveChangesAsync(cancellationToken);

        TempData["Mensagem"] = "Entrega cadastrada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BuscarEndereco(BuscarEnderecoRequest request, CancellationToken cancellationToken)
    {
        var cidadePermitida = ObterCidadePermitida(request.CidadeSelecionada);
        if (cidadePermitida is null)
        {
            return Json(new { sucesso = false, mensagem = "Selecione uma cidade válida." });
        }

        var resultado = await _googleMaps.BuscarEnderecoAsync(
            request.RuaPesquisada,
            request.Numero,
            cidadePermitida,
            cancellationToken);

        if (!resultado.Sucesso)
        {
            return Json(new { sucesso = false, mensagem = resultado.Mensagem });
        }

        return Json(new
        {
            sucesso = true,
            bairro = resultado.Bairro,
            cidade = resultado.Cidade,
            uf = resultado.Uf,
            pais = resultado.Pais,
            enderecoFormatado = resultado.EnderecoFormatado,
            latitude = resultado.Latitude,
            longitude = resultado.Longitude
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarcarSaida(int id, CancellationToken cancellationToken)
    {
        var entrega = await _context.Entregas.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entrega is not null && entrega.Status == EntregaStatus.AguardandoSaida)
        {
            entrega.Status = EntregaStatus.SaiuParaEntrega;
            entrega.SaiuParaEntregaEm = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            TempData["Mensagem"] = "Entrega marcada como saiu para entrega.";
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finalizar(int id, CancellationToken cancellationToken)
    {
        var entrega = await _context.Entregas.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        if (entrega is not null && entrega.Status != EntregaStatus.Finalizada)
        {
            entrega.Status = EntregaStatus.Finalizada;
            entrega.FinalizadaEm = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            TempData["Mensagem"] = "Entrega finalizada.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<EntregaIndexViewModel> CriarIndexViewModelAsync(
        EntregaCreateViewModel novaEntrega,
        CancellationToken cancellationToken)
    {
        novaEntrega.CidadesPermitidas = _googleMapsOptions.AllowedCities;

        var entregas = await _context.Entregas
            .AsNoTracking()
            .OrderBy(e => e.Status)
            .ThenByDescending(e => e.CriadoEm)
            .Take(100)
            .ToListAsync(cancellationToken);

        return new EntregaIndexViewModel
        {
            NovaEntrega = novaEntrega,
            Entregas = entregas
        };
    }

    private CidadePermitida? ObterCidadePermitida(string valor)
    {
        return _googleMapsOptions.AllowedCities
            .FirstOrDefault(c => string.Equals(c.Valor, valor, StringComparison.OrdinalIgnoreCase));
    }
}
