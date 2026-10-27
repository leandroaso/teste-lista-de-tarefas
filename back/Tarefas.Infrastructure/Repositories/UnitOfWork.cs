using Tarefas.Domain.Interfaces;

namespace Tarefas.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
	public UnitOfWork(ITarefaRepository tarefas)
	{
		Tarefas = tarefas;
	}

	public ITarefaRepository Tarefas { get; }
}
	