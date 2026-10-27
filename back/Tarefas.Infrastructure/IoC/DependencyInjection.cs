using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tarefas.Application.Interfaces;
using Tarefas.Application.Mappings;
using Tarefas.Application.Services;
using Tarefas.Application.Validators;
using Tarefas.Domain.Interfaces;
using Tarefas.Infrastructure.Context;
using Tarefas.Infrastructure.Repositories;

namespace Tarefas.Infrastructure.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddTarefas(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Sqlite");
        services.AddDbContext<AppDbContext>(o => o.UseSqlite(connectionString));

        services.AddScoped<ITarefaRepository, TarefaRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITarefaService, TarefaService>();

        services.AddAutoMapper(typeof(TarefaProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<TarefaRequestValidator>();
        return services;
    }
}

