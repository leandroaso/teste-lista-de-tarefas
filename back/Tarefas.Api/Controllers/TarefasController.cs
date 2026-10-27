using Microsoft.AspNetCore.Mvc;
using Tarefas.Application.DTOs;
using Tarefas.Application.Interfaces;
using Tarefas.Domain.Enums;

namespace Tarefas.Api.Controllers;

/// <summary>Gestão de tarefas.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TarefasController : ControllerBase
{
    private readonly ITarefaService _service;

    public TarefasController(ITarefaService service)
    {

        _service = service;
    }

    /// <summary>Lista tarefas com filtros opcionais.</summary>
    /// <param name="status">Pendente, EmAndamento ou Concluido.</param>
    /// <param name="vencimentoDe">Data de vencimento inicial (inclusiva).</param>
    /// <param name="vencimentoAte">Data de vencimento final (inclusiva).</param>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TarefaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar([FromQuery] StatusTarefa? status, [FromQuery] DateTime? vencimentoDe, [FromQuery] DateTime? vencimentoAte, CancellationToken cancellationToken)
    {

        return Ok(await _service.ListarAsync(new TarefaFiltroDto(status, vencimentoDe, vencimentoAte), cancellationToken));
    }

    /// <summary>Obtém uma tarefa por id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Obter(int id, CancellationToken cancellationToken)
    {

        return Ok(await _service.ObterPorIdAsync(id, cancellationToken));
    }

    /// <summary>Cria uma tarefa.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar(TarefaRequestDto dto, CancellationToken cancellationToken)
    {
        var criada = await _service.CriarAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(Obter), new { id = criada.Id }, criada);
    }

    /// <summary>Atualiza uma tarefa.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, TarefaRequestDto dto, CancellationToken cancellationToken)
    {

        return Ok(await _service.AtualizarAsync(id, dto, cancellationToken));
    }

    /// <summary>Altera somente o status da tarefa.</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(typeof(TarefaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarStatus(int id, AtualizarStatusDto dto, CancellationToken cancellationToken)
    {

        return Ok(await _service.AtualizarStatusAsync(id, dto, cancellationToken));
    }

    /// <summary>Remove uma tarefa.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken cancellationToken)
    {
        await _service.RemoverAsync(id, cancellationToken);
        return NoContent();
    }
}
