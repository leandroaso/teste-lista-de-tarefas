using Tarefas.Application.DTOs;

namespace Tarefas.Application.Interfaces;

public interface ITarefaService
{
    Task<IReadOnlyList<TarefaResponseDto>> ListarAsync(TarefaFiltroDto filtro, CancellationToken cancellationToken = default);
    Task<TarefaResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TarefaResponseDto> CriarAsync(TarefaRequestDto dto, CancellationToken cancellationToken = default);
    Task<TarefaResponseDto> AtualizarAsync(int id, TarefaRequestDto dto, CancellationToken cancellationToken = default);
    Task<TarefaResponseDto> AtualizarStatusAsync(int id, AtualizarStatusDto dto, CancellationToken cancellationToken = default);
    Task RemoverAsync(int id, CancellationToken cancellationToken = default);
}
