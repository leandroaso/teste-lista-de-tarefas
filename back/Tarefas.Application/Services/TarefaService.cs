using AutoMapper;
using FluentValidation;
using Tarefas.Application.DTOs;
using Tarefas.Application.Interfaces;
using Tarefas.Domain.Entities;
using Tarefas.Domain.Exceptions;
using Tarefas.Domain.Interfaces;

namespace Tarefas.Application.Services;

public class TarefaService : ITarefaService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IValidator<TarefaRequestDto> _requestValidator;
    private readonly IValidator<AtualizarStatusDto> _statusValidator;

    public TarefaService(IUnitOfWork uow, IValidator<TarefaRequestDto> requestValidator, IValidator<AtualizarStatusDto> statusValidator, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
        _requestValidator = requestValidator;
        _statusValidator = statusValidator;
    }

    public async Task<IReadOnlyList<TarefaResponseDto>> ListarAsync(TarefaFiltroDto filtro, CancellationToken cancellationToken = default)
    {
        var tarefas = await _uow.Tarefas.ListarAsync(filtro.Status, filtro.VencimentoDe, filtro.VencimentoAte, cancellationToken);
        return _mapper.Map<List<TarefaResponseDto>>(tarefas);
    }

    public async Task<TarefaResponseDto> ObterPorIdAsync(int id, CancellationToken cancellationToken = default) =>
        _mapper.Map<TarefaResponseDto>(await ObterOuFalharAsync(id, cancellationToken));

    public async Task<TarefaResponseDto> CriarAsync(TarefaRequestDto dto, CancellationToken cancellationToken = default)
    {
        await _requestValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var tarefa = _mapper.Map<Tarefa>(dto);
        await _uow.Tarefas.AdicionarAsync(tarefa, cancellationToken);
        return _mapper.Map<TarefaResponseDto>(tarefa);
    }

    public async Task<TarefaResponseDto> AtualizarAsync(int id, TarefaRequestDto dto, CancellationToken cancellationToken = default)
    {
        await _requestValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var tarefa = await ObterOuFalharAsync(id, cancellationToken);
        tarefa.Atualizar(dto.Titulo, dto.Descricao, dto.DataVencimento, dto.Status);
        await _uow.Tarefas.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TarefaResponseDto>(tarefa);
    }

    public async Task<TarefaResponseDto> AtualizarStatusAsync(int id, AtualizarStatusDto dto, CancellationToken cancellationToken = default)
    {
        await _statusValidator.ValidateAndThrowAsync(dto, cancellationToken);
        var tarefa = await ObterOuFalharAsync(id, cancellationToken);
        tarefa.AlterarStatus(dto.Status);
        await _uow.Tarefas.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TarefaResponseDto>(tarefa);
    }

    public async Task RemoverAsync(int id, CancellationToken cancellationToken = default)
    {
        var tarefa = await ObterOuFalharAsync(id, cancellationToken);
        await _uow.Tarefas.RemoverAsync(tarefa, cancellationToken);
    }

    private async Task<Tarefa> ObterOuFalharAsync(int id, CancellationToken cancellationToken)
    {
        return await _uow.Tarefas.ObterPorIdAsync(id, cancellationToken) ?? throw new NotFoundException($"Tarefa {id} não encontrada.");
    }
}

