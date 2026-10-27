using Tarefas.Domain.Enums;

namespace Tarefas.Application.DTOs;

public record TarefaRequestDto(string Titulo, string? Descricao, DateTime DataVencimento, StatusTarefa Status = StatusTarefa.Pendente);

public record AtualizarStatusDto(StatusTarefa Status);

public record TarefaFiltroDto(StatusTarefa? Status, DateTime? VencimentoDe, DateTime? VencimentoAte);

public record TarefaResponseDto(int Id, string Titulo, string? Descricao, StatusTarefa Status, DateTime DataVencimento, DateTime CriadoEm, DateTime? AtualizadoEm);
