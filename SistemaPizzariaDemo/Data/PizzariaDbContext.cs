using Microsoft.EntityFrameworkCore;
using SistemaPizzariaDemo.Models;

namespace SistemaPizzariaDemo.Data;

public class PizzariaDbContext : DbContext
{
    public PizzariaDbContext(DbContextOptions<PizzariaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Motoboy> Motoboys => Set<Motoboy>();

    public DbSet<Entrega> Entregas => Set<Entrega>();

    public DbSet<Configuracao> Configuracoes => Set<Configuracao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Motoboy>(entity =>
        {
            entity.ToTable("Motoboys");
            entity.Property(m => m.Nome).HasMaxLength(120).IsRequired();
            entity.Property(m => m.Telefone).HasMaxLength(30);
            entity.Property(m => m.Placa).HasMaxLength(15);
            entity.Property(m => m.Ativo).HasDefaultValue(true);
            entity.Property(m => m.Disponivel).HasDefaultValue(true);
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.ToTable("Entregas");
            entity.Property(e => e.NumeroPedido).HasMaxLength(30).IsRequired();
            entity.Property(e => e.RuaPesquisada).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Numero).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Complemento).HasMaxLength(120);
            entity.Property(e => e.Bairro).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Cidade).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Uf).HasMaxLength(2).IsRequired();
            entity.Property(e => e.Pais).HasMaxLength(80).IsRequired();
            entity.Property(e => e.EnderecoFormatado).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Status).HasConversion<int>();
        });

        modelBuilder.Entity<Configuracao>(entity =>
        {
            entity.ToTable("Configuracoes");
            entity.HasKey(c => c.Chave);
            entity.Property(c => c.Chave).HasMaxLength(80);
            entity.Property(c => c.Valor).HasMaxLength(500).IsRequired();
            entity.HasData(new Configuracao
            {
                Chave = ConfiguracaoChaves.MaxEntregasPorMotoboy,
                Valor = "3"
            });
        });
    }
}
