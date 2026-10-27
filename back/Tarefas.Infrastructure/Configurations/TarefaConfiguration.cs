using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tarefas.Domain.Entities;

namespace Tarefas.Infrastructure.Configurations;

public class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("Tarefas");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Titulo).IsRequired().HasMaxLength(Tarefa.TituloMaxLength);
        builder.Property(t => t.Descricao).HasMaxLength(Tarefa.DescricaoMaxLength);
        builder.Property(t => t.Status).HasConversion<int>().IsRequired();
        builder.Property(t => t.DataVencimento).IsRequired();
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.DataVencimento);
    }
}
