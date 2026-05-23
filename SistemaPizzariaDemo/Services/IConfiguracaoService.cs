namespace SistemaPizzariaDemo.Services;

public interface IConfiguracaoService
{
    Task<int> ObterLimiteMaximoEntregasPorMotoboyAsync(CancellationToken cancellationToken = default);

    Task SalvarLimiteMaximoEntregasPorMotoboyAsync(int limite, CancellationToken cancellationToken = default);
}
