using Prueba.Application.Interfaces.Servicie;
using Prueba.Application.Servicios;
using PruebaWebApi.Extensions;
using PruebaWebApi.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// 🔥 SERILOG
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddCore(builder.Configuration);
builder.Services.AddLoggingServices();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Inyección de dependencia
builder.Services.AddScoped<IUsuarioService, UsuarioServicie>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// 🔥 MIDDLEWARES
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<SimpleLogMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
