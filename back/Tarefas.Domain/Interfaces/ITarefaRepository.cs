using Tarefas.Domain.Entities;
using Tarefas.Domain.Enums;

namespace Tarefas.Domain.Interfaces;

public interface ITarefaRepository
{
    Task<IReadOnlyList<Tarefa>> ListarAsync(StatusTarefa? status, DateTime? vencimentoDe, DateTime? vencimentoAte, CancellationToken cancellationToken = default);
    Task<Tarefa?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task AdicionarAsync(Tarefa tarefa, CancellationToken cancellationToken = default);
    Task RemoverAsync(Tarefa tarefa, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
