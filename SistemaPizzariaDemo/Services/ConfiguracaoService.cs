using Microsoft.EntityFrameworkCore;
using SistemaPizzariaDemo.Data;
using SistemaPizzariaDemo.Models;

namespace SistemaPizzariaDemo.Services;

public class ConfiguracaoService : IConfiguracaoService
{
    private const int LimitePadrao = 3;
    private readonly PizzariaDbContext _context;

    public ConfiguracaoService(PizzariaDbContext context)
    {
        _context = context;
    }

    public async Task<int> ObterLimiteMaximoEntregasPorMotoboyAsync(CancellationToken cancellationToken = default)
    {
        var configuracao = await _context.Configuracoes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Chave == ConfiguracaoChaves.MaxEntregasPorMotoboy, cancellationToken);

        if (configuracao is null || !int.TryParse(configuracao.Valor, out var limite))
        {
            return LimitePadrao;
        }

        return Math.Max(1, limite);
    }

    public async Task SalvarLimiteMaximoEntregasPorMotoboyAsync(int limite, CancellationToken cancellationToken = default)
    {
        limite = Math.Max(1, limite);

        var configuracao = await _context.Configuracoes
            .FirstOrDefaultAsync(c => c.Chave == ConfiguracaoChaves.MaxEntregasPorMotoboy, cancellationToken);

        if (configuracao is null)
        {
            _context.Configuracoes.Add(new Configuracao
            {
                Chave = ConfiguracaoChaves.MaxEntregasPorMotoboy,
                Valor = limite.ToString()
            });
        }
        else
        {
            configuracao.Valor = limite.ToString();
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
