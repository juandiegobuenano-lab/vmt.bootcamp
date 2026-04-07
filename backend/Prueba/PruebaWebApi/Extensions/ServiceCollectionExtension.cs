using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Helpers;
using Prueba.Application.Interfaces.Servicie;
using Prueba.Application.Servicios;
using Prueba.Infrastructure.Persistence.SqlServer.Repositories;
using Prueba.Shared.Constants;
using PruebaDomain.Database.SqlServer.Context;
using PruebaDomain.Interfaces.Repositories;
using PruebaWebApi.Middleware;
using Serilog;

namespace PruebaWebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Método que sirve para añadir todos los servicios de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioServicie>();
        }

        /// <summary>
        /// Método que sirve para añadir todos los repositorios de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
        }

        /// <summary>
        /// Método que añade lo esencial que necesita nuestra aplicación para funcionar
        /// </summary>
        /// <param name="services"></param>
        public static void AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (ErrorContext) =>
                {
                    var errors = ErrorContext.ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    var response = ResponseHelper.Create(ValidationConstants.VALIDATION_MESSAGE, errors: errors, message: ValidationConstants.VALIDATION_MESSAGE);
                    return new BadRequestObjectResult(response);
                };
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            services.AddSqlServer<PruebaContext>(configuration.GetConnectionString("database-1"));
            services.AddRepositories();

            services.AddServices();

            services.AddMiddlewares();
        }

        /// <summary>
        /// Método que añade los middlewares de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlerMiddleware>();
        }

        public static void AddLoggingServices(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
