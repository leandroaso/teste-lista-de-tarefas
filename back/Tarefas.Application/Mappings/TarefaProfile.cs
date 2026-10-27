using AutoMapper;
using Tarefas.Application.DTOs;
using Tarefas.Domain.Entities;

namespace Tarefas.Application.Mappings;

public class TarefaProfile : Profile
{
    public TarefaProfile()
    {
        CreateMap<Tarefa, TarefaResponseDto>();

        CreateMap<TarefaRequestDto, Tarefa>()
            .ConstructUsing(d => new Tarefa(d.Titulo, d.Descricao, d.DataVencimento, d.Status))
            .ForAllMembers(o => o.Ignore());
    }
}
