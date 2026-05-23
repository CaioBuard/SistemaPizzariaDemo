using SistemaPizzariaDemo.Models;
using SistemaPizzariaDemo.Services;

namespace SistemaPizzariaDemo.Tests;

public class EntregaAgrupamentoServiceTests
{
    private readonly EntregaAgrupamentoService _service = new();

    [Fact]
    public void MontarPainel_UsaApenasMotoboysAtivosEDisponiveis()
    {
        var entregas = new List<Entrega>
        {
            CriarEntrega("1", "Centro"),
            CriarEntrega("2", "Boa Vista")
        };
        var motoboys = new List<Motoboy>
        {
            CriarMotoboy(1, "Ana", disponivel: true, ativo: true),
            CriarMotoboy(2, "Bruno", disponivel: false, ativo: true),
            CriarMotoboy(3, "Carlos", disponivel: true, ativo: false)
        };

        var painel = _service.MontarPainel(entregas, motoboys, limitePorMotoboy: 3);

        Assert.Equal(1, painel.TotalMotoboysDisponiveis);
        Assert.All(painel.Grupos.Where(g => !g.SemMotoboy), grupo => Assert.Equal(1, grupo.MotoboyId));
    }

    [Fact]
    public void MontarPainel_QuandoNaoHaCapacidade_CriaGrupoSemMotoboy()
    {
        var entregas = new List<Entrega>
        {
            CriarEntrega("1", "Boa Vista"),
            CriarEntrega("2", "Centro")
        };
        var motoboys = new List<Motoboy>
        {
            CriarMotoboy(1, "Ana", disponivel: true, ativo: true)
        };

        var painel = _service.MontarPainel(entregas, motoboys, limitePorMotoboy: 1);

        Assert.Contains(painel.Grupos, grupo => grupo.SemMotoboy);
        Assert.Single(painel.Grupos.Where(g => g.SemMotoboy));
    }

    [Fact]
    public void MontarPainel_QuandoBairroExcedeLimite_DivideEmBlocos()
    {
        var entregas = Enumerable.Range(1, 5)
            .Select(i => CriarEntrega(i.ToString(), "Centro"))
            .ToList();
        var motoboys = new List<Motoboy>
        {
            CriarMotoboy(1, "Ana", disponivel: true, ativo: true),
            CriarMotoboy(2, "Bruno", disponivel: true, ativo: true)
        };

        var painel = _service.MontarPainel(entregas, motoboys, limitePorMotoboy: 3);

        Assert.Equal(2, painel.Grupos.Count);
        Assert.Equal(new[] { 3, 2 }, painel.Grupos.Select(g => g.Entregas.Count).ToArray());
        Assert.All(painel.Grupos, grupo => Assert.Equal("Centro", grupo.Bairro));
    }

    [Fact]
    public void MontarPainel_AlternaCoresDosGruposComMotoboy()
    {
        var entregas = new List<Entrega>
        {
            CriarEntrega("1", "Boa Vista"),
            CriarEntrega("2", "Centro"),
            CriarEntrega("3", "Portao")
        };
        var motoboys = new List<Motoboy>
        {
            CriarMotoboy(1, "Ana", disponivel: true, ativo: true),
            CriarMotoboy(2, "Bruno", disponivel: true, ativo: true),
            CriarMotoboy(3, "Carlos", disponivel: true, ativo: true)
        };

        var painel = _service.MontarPainel(entregas, motoboys, limitePorMotoboy: 1);

        Assert.Equal(new[] { "grupo-laranja", "grupo-verde", "grupo-laranja" }, painel.Grupos.Select(g => g.CorClasse));
    }

    private static Entrega CriarEntrega(string numeroPedido, string bairro)
    {
        return new Entrega
        {
            NumeroPedido = numeroPedido,
            QuantidadePizzas = 1,
            RuaPesquisada = "Rua Teste",
            Numero = "10",
            Bairro = bairro,
            Cidade = "Curitiba",
            Uf = "PR",
            Pais = "Brasil",
            EnderecoFormatado = $"Rua Teste, 10 - {bairro}",
            Status = EntregaStatus.AguardandoSaida
        };
    }

    private static Motoboy CriarMotoboy(int id, string nome, bool disponivel, bool ativo)
    {
        return new Motoboy
        {
            Id = id,
            Nome = nome,
            Disponivel = disponivel,
            Ativo = ativo
        };
    }
}
