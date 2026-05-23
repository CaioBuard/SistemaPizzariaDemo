using Microsoft.EntityFrameworkCore;
using SistemaPizzariaDemo.Data;
using SistemaPizzariaDemo.Models;
using SistemaPizzariaDemo.Services;

namespace SistemaPizzariaDemo.Tests;

public class MotoboyServiceTests
{
    [Fact]
    public async Task DesativarAsync_InativaSemRemoverRegistro()
    {
        await using var context = CriarContexto();
        context.Motoboys.Add(new Motoboy
        {
            Id = 1,
            Nome = "Ana",
            Ativo = true,
            Disponivel = true
        });
        await context.SaveChangesAsync();

        var service = new MotoboyService(context);

        var alterado = await service.DesativarAsync(1);
        var motoboy = await context.Motoboys.IgnoreQueryFilters().FirstAsync(m => m.Id == 1);

        Assert.True(alterado);
        Assert.False(motoboy.Ativo);
        Assert.False(motoboy.Disponivel);
        Assert.NotNull(motoboy.InativadoEm);
        Assert.Equal(1, await context.Motoboys.CountAsync());
    }

    [Fact]
    public async Task AlternarDisponibilidadeAsync_AlternaStatusEPersiste()
    {
        await using var context = CriarContexto();
        context.Motoboys.Add(new Motoboy
        {
            Id = 1,
            Nome = "Ana",
            Ativo = true,
            Disponivel = true
        });
        await context.SaveChangesAsync();

        var service = new MotoboyService(context);

        var alterado = await service.AlternarDisponibilidadeAsync(1);
        var motoboy = await context.Motoboys.FirstAsync(m => m.Id == 1);

        Assert.True(alterado);
        Assert.False(motoboy.Disponivel);
    }

    private static PizzariaDbContext CriarContexto()
    {
        var options = new DbContextOptionsBuilder<PizzariaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PizzariaDbContext(options);
    }
}
