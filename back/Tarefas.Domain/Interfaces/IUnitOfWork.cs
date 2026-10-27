namespace Tarefas.Domain.Interfaces;

public interface IUnitOfWork
{
    ITarefaRepository Tarefas { get; }
}
