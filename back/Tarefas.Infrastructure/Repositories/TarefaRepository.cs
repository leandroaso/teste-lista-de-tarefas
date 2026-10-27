using Microsoft.EntityFrameworkCore;
using Tarefas.Domain.Entities;
using Tarefas.Domain.Enums;
using Tarefas.Domain.Interfaces;
using Tarefas.Infrastructure.Context;

namespace Tarefas.Infrastructure.Repositories;

public class TarefaRepository : ITarefaRepository
{
    private readonly AppDbContext _context;

    public TarefaRepository(AppDbContext context)
    {

        _context = context;
    }

    public async Task<IReadOnlyList<Tarefa>> ListarAsync(StatusTarefa? status, DateTime? vencimentoDe, DateTime? vencimentoAte, CancellationToken cancellationToken = default)
    {
        var query = _context.Tarefas.AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (vencimentoDe.HasValue)
            query = query.Where(t => t.DataVencimento >= vencimentoDe.Value.Date);

        if (vencimentoAte.HasValue)
            query = query.Where(t => t.DataVencimento < vencimentoAte.Value.Date.AddDays(1));

        return await query.OrderBy(t => t.DataVencimento).ThenBy(t => t.Id).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Tarefa?> ObterPorIdAsync(int id, CancellationToken cancellationToken = default)
    {

        return await _context.Tarefas.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task AdicionarAsync(Tarefa tarefa, CancellationToken cancellationToken = default)
    {
        await _context.Tarefas.AddAsync(tarefa, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoverAsync(Tarefa tarefa, CancellationToken cancellationToken = default)
    {
        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
