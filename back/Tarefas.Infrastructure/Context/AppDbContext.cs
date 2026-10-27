using Microsoft.EntityFrameworkCore;
using Tarefas.Domain.Entities;
using Tarefas.Infrastructure.Configurations;

namespace Tarefas.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Tarefa> Tarefas => Set<Tarefa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfiguration(new TarefaConfiguration());
}
