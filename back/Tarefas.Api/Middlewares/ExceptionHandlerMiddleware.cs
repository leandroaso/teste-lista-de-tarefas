using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Tarefas.Domain.Exceptions;

namespace Tarefas.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var status = 500;
            var title = "Erro interno no servidor.";
            var errors = new Dictionary<string, string[]>();

            switch (ex)
            {
                case ValidationException validationException:
                    status = 400;
                    title = "Dados inválidos.";
                    errors = validationException.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
                    break;

                case DomainException domainException:
                    status = 400;
                    title = domainException.Message;
                    break;

                case NotFoundException notFoundException:
                    status = 404;
                    title = notFoundException.Message;
                    break;

                default:
                    break;
            }

            if (status == 500)
                _logger.LogError(ex, "Erro não tratado");

            var problem = new ValidationProblemDetails(errors ?? new Dictionary<string, string[]>())
            {
                Status = status,
                Title = title
            };
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problem);
        }
    }
}
