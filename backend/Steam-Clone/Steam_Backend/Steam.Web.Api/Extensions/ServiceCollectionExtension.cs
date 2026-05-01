using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Steam.Web.Api.Middleware;
using SteamApplication.Helpers;
using SteamApplication.Interfaces.Servicie;
using SteamApplication.Servicios;
using SteamApplication.Servicios.EmailTemplates;
using SteamDomain.Database.SqlServer;
using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Exceptions;
using SteamDomain.Interfaces.Repositories;
using SteamInfrastructure.Persistence.SqlServer;
using SteamInfrastructure.Persistence.SqlServer.Repositories;
using SteamShared.Constants;
using SteamShared.Helpers;
using System.ComponentModel.DataAnnotations;



namespace Steam.Web.Api.Extensions
{
    public static class ServiceCollectionExtension
    {

        /// <summary>
        /// Método que sirve para añadir todos los servicios de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IAppService, AppService>();
        }

        /// <summary>
        /// Método que sirve para añadir todos los repositorios de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
        }

        public static Task addSTMP(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("Smtp");
            services.Configure<SmtpSettings>(section);

            var smtpSettings = section.Get<SmtpSettings>();
            if (smtpSettings == null)
                throw new Exception("Sección 'Smtp' no encontrada en la configuración.");

            var context = new ValidationContext(smtpSettings, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(smtpSettings, context, results, true))
            {
                var messages = string.Join("; ", results.Select(r => r.ErrorMessage));
                throw new Exception($"Faltan propiedades de configuración Smtp: {messages}");
            }

            var smtp = new SMTP(smtpSettings.Host, smtpSettings.From, smtpSettings.Port, smtpSettings.User, smtpSettings.Password);
            services.AddSingleton(smtp);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Método que añade lo esencial que necesita nuestra aplicación para funcionar
        /// </summary>
        /// <param name="services"></param>
        public static async Task AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            await services.addSTMP(configuration);

            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = (errorContext) =>
                {
                    var errors = errorContext.ModelState.Values.SelectMany(value => value.Errors.Select(error => error.ErrorMessage).ToList()).ToList();
                    var response = ResponseHelper.Create(
                        data: ValidationConstants.VALIDATION_MESSAGE,
                        errors: errors,
                        message: ValidationConstants.VALIDATION_MESSAGE
                        );
                    return new BadRequestObjectResult(response);
                };
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            var databaseConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__database-1")
    ?? configuration.GetConnectionString("database-1");

            if (string.IsNullOrWhiteSpace(databaseConnectionString))
                throw new InvalidOperationException($"Falta la cadena de conexión: {ConfigurationConstants.CONNECTION_STRING_DATABASE}");

            services.AddSqlServer<SteamContext>(databaseConnectionString);
            services.AddRepositories();

            services.AddServices();

            services.AddMiddlewares();
            services.AddLogging();

            services.AddAuth(configuration);
            services.AddCache();

        }

        /// <summary>
        /// Método que añade los middlewares de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandlerMiddleware>();
        }



        /// <summary>
        /// Método para añadir todo lo relacionado al logging
        /// </summary>
        /// <param name="services"></param>
        public static void AddLogging(this IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt"), rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();


        }

        public async static Task Initialize(this IServiceCollection services)
        {
            var templatesData = new EmailTemplateData();
            services.AddSingleton(templatesData);

            var provider = services.BuildServiceProvider();
            var scope = provider.CreateAsyncScope();

            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            await userService.CreateFirstUser();

            var emailTemplateService = scope.ServiceProvider.GetRequiredService<IEmailTemplateService>();
            await emailTemplateService.Init();
        }

        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(builder =>
            {
                builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(builder =>
            {

                var tokenConfiguration = TokenHelper.Configuration(configuration);

                var issuer = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_ISSUER)
                    ?? configuration[ConfigurationConstants.JWT_ISSUER]
                    ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_ISSUER));

                var audience = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_AUDIENCE)
                    ?? configuration[ConfigurationConstants.JWT_AUDIENCE]
                    ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_AUDIENCE));

                var privateKey = Environment.GetEnvironmentVariable(ConfigurationConstants.JWT_PRIVATE_KEY)
                    ?? configuration[ConfigurationConstants.JWT_PRIVATE_KEY]
                    ?? throw new Exception(ResponseConstants.ConfigurationPropertyNotFound(ConfigurationConstants.JWT_PRIVATE_KEY));

                builder.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenConfiguration.SecurityKey,
                    ClockSkew = TimeSpan.Zero
                };

                builder.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        throw new UnauthorizedException(ResponseConstants.AUTH_TOKEN_NOT_FOUND);
                    }
                };
            });

            services.AddAuthorization();
        }

        public static void AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
        }

    }
}

