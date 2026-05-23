namespace SistemaPizzariaDemo.Services;

public interface IMotoboyService
{
    Task<bool> DesativarAsync(int id, CancellationToken cancellationToken = default);

    Task<bool> AlternarDisponibilidadeAsync(int id, CancellationToken cancellationToken = default);
}
