using Microsoft.EntityFrameworkCore;
using SistemaPizzariaDemo.Data;

namespace SistemaPizzariaDemo.Services;

public class MotoboyService : IMotoboyService
{
    private readonly PizzariaDbContext _context;

    public MotoboyService(PizzariaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DesativarAsync(int id, CancellationToken cancellationToken = default)
    {
        var motoboy = await _context.Motoboys.FirstOrDefaultAsync(m => m.Id == id && m.Ativo, cancellationToken);

        if (motoboy is null)
        {
            return false;
        }

        motoboy.Ativo = false;
        motoboy.Disponivel = false;
        motoboy.InativadoEm = DateTime.Now;
        motoboy.AtualizadoEm = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> AlternarDisponibilidadeAsync(int id, CancellationToken cancellationToken = default)
    {
        var motoboy = await _context.Motoboys.FirstOrDefaultAsync(m => m.Id == id && m.Ativo, cancellationToken);

        if (motoboy is null)
        {
            return false;
        }

        motoboy.Disponivel = !motoboy.Disponivel;
        motoboy.AtualizadoEm = DateTime.Now;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
