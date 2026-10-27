using Tarefas.Domain.Entities;
using Tarefas.Domain.Enums;
using Tarefas.Domain.Exceptions;
using Xunit;

namespace Tarefas.Application.Test.Entities;

public class TarefaTests
{
    [Fact]
    public void Criar_ComTituloVazio_Lanca()
    {
        Assert.Throws<DomainException>(() => new Tarefa(" ", null, DateTime.Today));
    }

    [Fact]
    public void Criar_DefineStatusPendenteEAparaTitulo()
    {
        var t = new Tarefa("  Estudar ", "", DateTime.Today);
        Assert.Equal("Estudar", t.Titulo);
        Assert.Null(t.Descricao);
        Assert.Equal(StatusTarefa.Pendente, t.Status);
    }

    [Fact]
    public void AlterarStatus_Invalido_Lanca()
    {
        var t = new Tarefa("a", null, DateTime.Today);
        Assert.Throws<DomainException>(() => t.AlterarStatus((StatusTarefa)99));
    }
}
