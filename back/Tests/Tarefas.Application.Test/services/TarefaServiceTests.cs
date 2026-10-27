using AutoMapper;
using FluentValidation;
using NSubstitute;
using Tarefas.Application.DTOs;
using Tarefas.Application.Mappings;
using Tarefas.Application.Services;
using Tarefas.Application.Validators;
using Tarefas.Domain.Entities;
using Tarefas.Domain.Enums;
using Tarefas.Domain.Exceptions;
using Tarefas.Domain.Interfaces;
using Xunit;

namespace Tarefas.Application.Test.services;

public class TarefaServiceTests
{
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly ITarefaRepository _repo = Substitute.For<ITarefaRepository>();
    private readonly TarefaService _service;

    public TarefaServiceTests()
    {
        _uow.Tarefas.Returns(_repo);
        _service = new TarefaService(_uow, new TarefaRequestValidator(), new AtualizarStatusValidator(),
            new MapperConfiguration(c => c.AddProfile<TarefaProfile>()).CreateMapper());
    }

    [Fact]
    public async Task Criar_Valido_PersisteEFazCommit()
    {
        var r = await _service.CriarAsync(new TarefaRequestDto("Tarefa", "d", DateTime.Today.AddDays(1)));

        await _repo.Received(1).AdicionarAsync(Arg.Any<Tarefa>(), Arg.Any<CancellationToken>());
        Assert.Equal("Tarefa", r.Titulo);
    }

    [Fact]
    public async Task Criar_TituloVazio_LancaValidationException()
    {
        await Assert.ThrowsAsync<ValidationException>(() =>
            _service.CriarAsync(new TarefaRequestDto("", null, DateTime.Today)));
    }

    [Fact]
    public async Task AtualizarStatus_Inexistente_LancaNotFound()
    {
        _repo.ObterPorIdAsync(1, Arg.Any<CancellationToken>()).Returns((Tarefa?)null);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.AtualizarStatusAsync(1, new AtualizarStatusDto(StatusTarefa.Concluido)));
    }

    [Fact]
    public async Task AtualizarStatus_Existente_AlteraStatus()
    {
        var t = new Tarefa("a", null, DateTime.Today);
        _repo.ObterPorIdAsync(1, Arg.Any<CancellationToken>()).Returns(t);

        var r = await _service.AtualizarStatusAsync(1, new AtualizarStatusDto(StatusTarefa.EmAndamento));

        Assert.Equal(StatusTarefa.EmAndamento, r.Status);
    }

    [Fact]
    public async Task Remover_Existente_Remove()
    {
        var t = new Tarefa("a", null, DateTime.Today);
        _repo.ObterPorIdAsync(1, Arg.Any<CancellationToken>()).Returns(t);

        await _service.RemoverAsync(1);
    }
}

