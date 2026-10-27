using Tarefas.Domain.Common;
using Tarefas.Domain.Enums;
using Tarefas.Domain.Exceptions;

namespace Tarefas.Domain.Entities;

public class Tarefa : BaseEntity
{
    public const int TituloMaxLength = 200;
    public const int DescricaoMaxLength = 1000;

    public string Titulo { get; private set; } = string.Empty;
    public string? Descricao { get; private set; }
    public StatusTarefa Status { get; private set; }
    public DateTime DataVencimento { get; private set; }

    private Tarefa() { }

    public Tarefa(string titulo, string? descricao, DateTime dataVencimento, StatusTarefa status = StatusTarefa.Pendente)
    {
        Preencher(titulo, descricao, dataVencimento);
        ValidarStatus(status);
        Status = status;
    }

    public void Atualizar(string titulo, string? descricao, DateTime dataVencimento, StatusTarefa status)
    {
        Preencher(titulo, descricao, dataVencimento);
        ValidarStatus(status);
        Status = status;
        AtualizadoEm = DateTime.UtcNow;
    }

    public void AlterarStatus(StatusTarefa status)
    {
        ValidarStatus(status);
        Status = status;
        AtualizadoEm = DateTime.UtcNow;
    }

    private static void ValidarStatus(StatusTarefa status)
    {
        if (!Enum.IsDefined(status))
            throw new DomainException("Status inválido.");
    }

    private void Preencher(string titulo, string? descricao, DateTime dataVencimento)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new DomainException("O título é obrigatório.");
        if (titulo.Trim().Length > TituloMaxLength)
            throw new DomainException($"O título deve ter no máximo {TituloMaxLength} caracteres.");
        if (descricao is { Length: > DescricaoMaxLength })
            throw new DomainException($"A descrição deve ter no máximo {DescricaoMaxLength} caracteres.");

        Titulo = titulo.Trim();
        Descricao = string.IsNullOrWhiteSpace(descricao) ? null : descricao.Trim();
        DataVencimento = dataVencimento;
    }
}
