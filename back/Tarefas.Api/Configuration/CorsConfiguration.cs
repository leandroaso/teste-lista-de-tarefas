using System.Runtime.CompilerServices;

namespace Tarefas.Api.Configuration
{
    public static class CorsConfiguration
    {

        public static IServiceCollection AddCorsOrigins(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration.GetSection("Cors:Origins").Get<string[]>() ?? ["http://localhost:4200"];

            services.AddCors(o => o.AddDefaultPolicy(p => p
                .WithOrigins(origins)
                .AllowAnyHeader()
                .AllowAnyMethod()));

            return services;
        }
    }
}
