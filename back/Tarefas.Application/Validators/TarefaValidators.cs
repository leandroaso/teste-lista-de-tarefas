using FluentValidation;
using Tarefas.Application.DTOs;
using Tarefas.Domain.Entities;

namespace Tarefas.Application.Validators;

public class TarefaRequestValidator : AbstractValidator<TarefaRequestDto>
{
    public TarefaRequestValidator()
    {
        RuleFor(x => x.Titulo).NotEmpty().MaximumLength(Tarefa.TituloMaxLength);
        RuleFor(x => x.Descricao).MaximumLength(Tarefa.DescricaoMaxLength);
        RuleFor(x => x.DataVencimento).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}

public class AtualizarStatusValidator : AbstractValidator<AtualizarStatusDto>
{
    public AtualizarStatusValidator() => RuleFor(x => x.Status).IsInEnum();
}
