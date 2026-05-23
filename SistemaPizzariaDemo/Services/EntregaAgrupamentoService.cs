using SistemaPizzariaDemo.Models;
using SistemaPizzariaDemo.ViewModels;

namespace SistemaPizzariaDemo.Services;

public class EntregaAgrupamentoService : IEntregaAgrupamentoService
{
    public PainelEntregasViewModel MontarPainel(
        IReadOnlyList<Entrega> entregas,
        IReadOnlyList<Motoboy> motoboys,
        int limitePorMotoboy)
    {
        var limite = Math.Max(1, limitePorMotoboy);
        var motoboysDisponiveis = motoboys
            .Where(m => m.Ativo && m.Disponivel)
            .OrderBy(m => m.Nome)
            .ToList();

        var entregasPendentes = entregas
            .Where(e => e.Status == EntregaStatus.AguardandoSaida)
            .OrderBy(e => e.Bairro)
            .ThenBy(e => e.Cidade)
            .ThenBy(e => e.NumeroPedido)
            .ToList();

        var grupos = new List<GrupoEntregasViewModel>();
        var entregasPorMotoboy = motoboysDisponiveis.ToDictionary(m => m.Id, _ => 0);
        var indiceMotoboy = 0;
        var gruposComMotoboy = 0;

        var bairros = entregasPendentes
            .GroupBy(e => new
            {
                Bairro = Normalizar(e.Bairro),
                Cidade = Normalizar(e.Cidade),
                Uf = Normalizar(e.Uf)
            })
            .OrderBy(g => g.Key.Bairro)
            .ThenBy(g => g.Key.Cidade)
            .ThenBy(g => g.Key.Uf);

        foreach (var bairro in bairros)
        {
            var entregasDoBairro = bairro
                .OrderBy(e => e.NumeroPedido)
                .ToArray();

            foreach (var bloco in entregasDoBairro.Chunk(limite))
            {
                var motoboy = EncontrarMotoboyDisponivel(
                    motoboysDisponiveis,
                    entregasPorMotoboy,
                    bloco.Length,
                    limite,
                    ref indiceMotoboy);

                var semMotoboy = motoboy is null;
                var corClasse = "grupo-sem-motoboy";

                if (motoboy is not null)
                {
                    entregasPorMotoboy[motoboy.Id] += bloco.Length;
                    corClasse = gruposComMotoboy % 2 == 0 ? "grupo-laranja" : "grupo-verde";
                    gruposComMotoboy++;
                }

                grupos.Add(new GrupoEntregasViewModel
                {
                    Ordem = grupos.Count + 1,
                    MotoboyId = motoboy?.Id,
                    MotoboyNome = motoboy?.Nome ?? "Sem motoboy disponível",
                    SemMotoboy = semMotoboy,
                    Bairro = bairro.Key.Bairro,
                    Cidade = bairro.Key.Cidade,
                    Uf = bairro.Key.Uf,
                    CorClasse = corClasse,
                    Entregas = bloco
                });
            }
        }

        return new PainelEntregasViewModel
        {
            LimitePorMotoboy = limite,
            TotalEntregasPendentes = entregasPendentes.Count,
            TotalMotoboysDisponiveis = motoboysDisponiveis.Count,
            Grupos = grupos
        };
    }

    private static Motoboy? EncontrarMotoboyDisponivel(
        IReadOnlyList<Motoboy> motoboys,
        IReadOnlyDictionary<int, int> entregasPorMotoboy,
        int quantidadeEntregas,
        int limite,
        ref int indiceMotoboy)
    {
        if (motoboys.Count == 0)
        {
            return null;
        }

        for (var tentativa = 0; tentativa < motoboys.Count; tentativa++)
        {
            var indiceAtual = (indiceMotoboy + tentativa) % motoboys.Count;
            var candidato = motoboys[indiceAtual];
            var usadas = entregasPorMotoboy[candidato.Id];

            if (usadas + quantidadeEntregas > limite)
            {
                continue;
            }

            indiceMotoboy = (indiceAtual + 1) % motoboys.Count;
            return candidato;
        }

        return null;
    }

    private static string Normalizar(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? "Não informado" : valor.Trim();
    }
}
