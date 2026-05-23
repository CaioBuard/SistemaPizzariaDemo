using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaPizzariaDemo.Data;
using SistemaPizzariaDemo.Models;
using SistemaPizzariaDemo.Services;

namespace SistemaPizzariaDemo.Controllers;

public class MotoboysController : Controller
{
    private readonly PizzariaDbContext _context;
    private readonly IMotoboyService _motoboyService;

    public MotoboysController(PizzariaDbContext context, IMotoboyService motoboyService)
    {
        _context = context;
        _motoboyService = motoboyService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var motoboys = await _context.Motoboys
            .AsNoTracking()
            .Where(m => m.Ativo)
            .OrderBy(m => m.Nome)
            .ToListAsync(cancellationToken);

        return View(motoboys);
    }

    public IActionResult Create()
    {
        return View(new Motoboy { Disponivel = true });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Motoboy motoboy, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(motoboy);
        }

        motoboy.Ativo = true;
        motoboy.Disponivel = true;
        motoboy.CriadoEm = DateTime.Now;

        _context.Motoboys.Add(motoboy);
        await _context.SaveChangesAsync(cancellationToken);

        TempData["Mensagem"] = "Motoboy cadastrado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var motoboy = await _context.Motoboys
            .FirstOrDefaultAsync(m => m.Id == id && m.Ativo, cancellationToken);

        if (motoboy is null)
        {
            return NotFound();
        }

        return View(motoboy);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Motoboy form, CancellationToken cancellationToken)
    {
        if (id != form.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(form);
        }

        var motoboy = await _context.Motoboys
            .FirstOrDefaultAsync(m => m.Id == id && m.Ativo, cancellationToken);

        if (motoboy is null)
        {
            return NotFound();
        }

        motoboy.Nome = form.Nome.Trim();
        motoboy.Telefone = form.Telefone;
        motoboy.Placa = form.Placa;
        motoboy.Disponivel = form.Disponivel;
        motoboy.AtualizadoEm = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);

        TempData["Mensagem"] = "Motoboy atualizado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AlternarDisponibilidade(int id, CancellationToken cancellationToken)
    {
        var alterado = await _motoboyService.AlternarDisponibilidadeAsync(id, cancellationToken);
        TempData["Mensagem"] = alterado
            ? "Disponibilidade atualizada."
            : "Motoboy não encontrado.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Excluir(int id, CancellationToken cancellationToken)
    {
        var alterado = await _motoboyService.DesativarAsync(id, cancellationToken);
        TempData["Mensagem"] = alterado
            ? "Motoboy inativado com sucesso."
            : "Motoboy não encontrado.";

        return RedirectToAction(nameof(Index));
    }
}
