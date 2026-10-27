namespace Tarefas.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; protected set; }
    public DateTime CriadoEm { get; protected set; } = DateTime.UtcNow;
    public DateTime? AtualizadoEm { get; protected set; }
}
